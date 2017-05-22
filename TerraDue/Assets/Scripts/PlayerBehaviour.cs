using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : LifeBehaviour
{
    [SyncVar]
    private float DeadSince;

    public PlayerBehaviour() : base(Constants.HERO_HEALTH, 0) {
    }

    protected override void Start()
    {
        base.Start();
        if (isLocalPlayer)
        {
            GameObject.Find("UI").GetComponent<MouseClick>().player = gameObject;
        }
        if (isServer)
        {
            DeadSince = -1;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isServer)
        {
            return;
        }
        // If dead check if time to respawn
        if (DeadSince >= 0)
        {
            DeadSince += Time.deltaTime;
            if (DeadSince >= Constants.RESPAWN_AFTER_SECS)
            {
                Health = MaximumHealth;
                DeadSince = -1;
                RpcRespawn();
            }
        }
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (!isServer)
        {
            return;
        }
        if (IsDead())
        {
            DeadSince = 0;
            RpcDie();
        }
    }

    protected override void UpdateHealth(float NewHealth)
    {
        // Update UI
        if (isLocalPlayer)
        {
            GameObject.Find("UI/Panel/Health/Bar").transform.localScale = new Vector3(NewHealth / MaximumHealth, 1f, 1f);
        }
    }

    [ClientRpc]
    private void RpcDie()
    {
        SetHeroVisible(false);
        if (isLocalPlayer)
        {
            GetComponent<MoveToPoint>().Hide();
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        // Show character
        SetHeroVisible(true);
        if (isLocalPlayer)
        {
            GetComponent<MoveToPoint>().Spawn();
        }
    }

    private void SetHeroVisible(bool visible)
    {
        gameObject.GetComponent<Renderer>().enabled = visible;
        transform.Find("HealthBar").GetComponent<Canvas>().enabled = visible;
    }

    [Command]
    public void CmdBuildTower(GameObject slot)
    {
        // Load the particles prefab from the network manager
        var prefab = GameObject.Find("LobbyManager").GetComponent<MyLobbyManager>().GetTowerPrefab(tag == "Human");
        GameObject instance = Instantiate(prefab, slot.transform);
        NetworkServer.Spawn(instance);
    }

    [Command]
    public void CmdCastAbility(GameObject target)
    {
        var prefab = GameObject.Find("LobbyManager").GetComponent<MyLobbyManager>().GetAbilityPrefab(tag == "Human");
        GameObject instance = Instantiate(prefab, target.transform);
        NetworkServer.Spawn(instance);
        StartCoroutine(AsyncTakeDamage(target, instance));
    }

    private IEnumerator AsyncTakeDamage(GameObject target, GameObject particles)
    {
        // Withdraw life after particle system
        yield return new WaitForSeconds(Constants.ABILITY_PARTICLES_DURATION);
        target.GetComponent<LifeBehaviour>().TakeDamage(Constants.ABILITY_DAMAGE);
        NetworkServer.Destroy(particles);
    }
}
