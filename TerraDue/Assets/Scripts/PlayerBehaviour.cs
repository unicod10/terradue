using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerBehaviour : LifeBehaviour
{
    private float deadSince;
    private float experience;

    public PlayerBehaviour() : base(Constants.HERO_BASE_HEALTH, 0) {
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
            deadSince = -1;
            experience = 0;
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
        if (deadSince >= 0)
        {
            deadSince += Time.deltaTime;
            if (deadSince >= Constants.RESPAWN_AFTER_SECS)
            {
                Health = MaximumHealth;
                RpcUpdateHealth(Health, MaximumHealth);
                deadSince = -1;
                RpcRespawn();
            }
        }
    }

    public override float TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (!isServer)
        {
            return 0;
        }
        if (IsDead())
        {
            deadSince = 0;
            RpcDie();
            // TODO level
            return Constants.HERO_BASE_EXPERIENCE;
        }
        return 0;
    }

    public void AddExperience(float experience)
    {
        this.experience += experience;
        RpcSetExperience(this.experience);

        // Increase life according to level
        float lifeRatio = Health / MaximumHealth;
        MaximumHealth = Constants.HERO_BASE_HEALTH * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        Health = lifeRatio * MaximumHealth;
        RpcUpdateHealth(Health, MaximumHealth);
    }

    protected override void UpdateHealth(float newHealth, float newMaximumHealth)
    {
        // Update UI
        if (isLocalPlayer)
        {
            GameObject.Find("UI/Panel/Health/Bar").transform.localScale = new Vector3(newHealth / newMaximumHealth, 1f, 1f);
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

    [ClientRpc]
    private void RpcSetExperience(float experience)
    {
        if (isLocalPlayer)
        {
            this.experience = experience;
            GameObject.Find("UI/Panel/Level").GetComponent<Text>().text = GetLevel().ToString();
        }
    }

    private int GetLevel()
    {
        int level = 0;
        float calcExperience = experience;
        do
        {
            level++;
            float neededExperience = Constants.HERO_LEVELUP_EXPERIENCE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, level - 1);
            calcExperience -= neededExperience;
        }
        while (calcExperience >= 0) ;
        return level;
    }

    [Command]
    public void CmdAttack(GameObject target)
    {
        var damage = Constants.HERO_ATTACK_BASE_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, damage);
    }

    [Command]
    public void CmdCastAbility(GameObject target)
    {
        var damage = Constants.HERO_ABILITY_BASE_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().CastAbility(gameObject, target, damage);
    }

    [Command]
    public void CmdBuildTower(GameObject slot)
    {
        GameObject.Find("ServerObject").GetComponent<TowersManager>().BuildTower(slot, tag == "Human");
    }
}
