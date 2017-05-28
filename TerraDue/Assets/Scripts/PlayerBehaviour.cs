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
            GameObject.Find("UI").GetComponent<UserInteraction>().player = gameObject;
            transform.Find("HealthBar").transform.position = new Vector3(1000, 1000, 1000);
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
                RpcUpdateHealth(Health);
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
    public void CmdAttack(GameObject target)
    {
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, Constants.HERO_ATTACK_DAMAGE);
    }

    [Command]
    public void CmdCastAbility(GameObject target)
    {
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().CastAbility(gameObject, target, Constants.ABILITY_DAMAGE);
    }

    [Command]
    public void CmdBuildTower(GameObject slot)
    {
        GameObject.Find("ServerObject").GetComponent<TowersManager>().BuildTower(slot, tag == "Human");
    }
}
