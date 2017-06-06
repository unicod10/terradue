using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerBehaviour : LifeBehaviour
{
    private float deadSince;
    private float experience;
    private int level;
	public AudioClip attackSound;
	public AudioClip castAbilitySound;
	public AudioClip levelUpSound;
	public AudioClip spawnSound;
	public AudioClip damageSound;
	public AudioClip buildTowerSound;

    public PlayerBehaviour() : base(Constants.HERO_BASE_HEALTH, 0) {
    }

    protected override void Start()
    {
        base.Start();
        if (isLocalPlayer)
        {
            var ui = GameObject.Find("UI");
            ui.GetComponent<UserInteraction>().player = gameObject;
            ui.transform.Find("Panel/Experience/Bar").transform.localScale = new Vector3(0, 1, 1);
            ui.GetComponent<SoundManager>().PlaySpawn();
            transform.Find("HealthBar").transform.position = new Vector3(1000, 1000, 1000);
        }
        if (isServer)
        {
            deadSince = -1;
            experience = 0;
            level = 1;
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
        RpcPlayTakeDamage();
        if (IsDead())
        {
            deadSince = 0;
            RpcDie();
            return Constants.HERO_BASE_EXPERIENCE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
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
            var ui = GameObject.Find("UI");
            ui.GetComponent<UserInteraction>().SetDefaultMessage();
            ui.GetComponent<SoundManager>().PlaySpawn();
        }
    }

    private void SetHeroVisible(bool visible)
    {
        GetComponentInChildren<Renderer>().enabled = visible;
        transform.Find("HealthBar").GetComponent<Canvas>().enabled = visible;
    }

    [ClientRpc]
    private void RpcSetExperience(float experience)
    {
        if (isLocalPlayer)
        {
            this.experience = experience;
            int level = GetLevel();
            GameObject.Find("UI/Panel/Level").GetComponent<Text>().text = level.ToString();
            float ratio = GetUnusedExperience() / GetNeededExperience(level);
            GameObject.Find("UI/Panel/Experience/Bar").transform.localScale = new Vector3(ratio, 1, 1);
        }
    }

    public int GetLevel()
    {
        int oldLevel = level;
        this.level = 0;
        float calcExperience = experience;
        do
        {
            this.level++; 
            float neededExperience = Constants.HERO_BASE_LEVELUP_EXP * Mathf.Pow(1 + Constants.BALANCING_INTEREST, this.level - 1);
            calcExperience -= neededExperience;
        }
        while (calcExperience >= 0);

        if(this.level > oldLevel)
        {
            GameObject.Find("UI").GetComponent<SoundManager>().PlayLevelUp();
        }
        return this.level;
    }

    public float GetUnusedExperience()
    {
        float calcExperience = experience;
        int level = 1;
        do
        {
            float neededExperience = Constants.HERO_BASE_LEVELUP_EXP * Mathf.Pow(1 + Constants.BALANCING_INTEREST, level - 1);
            if(calcExperience - neededExperience < 0)
            {
                return calcExperience;
            }
            calcExperience -= neededExperience;
            level++;
        }
        while (true);
    }

    public float GetNeededExperience(int currentLevel)
    {
        return Constants.HERO_BASE_LEVELUP_EXP * Mathf.Pow(1 + Constants.BALANCING_INTEREST, currentLevel - 1);
    }

    public float CalcHealRatio()
    {
        return Constants.HERO_BASE_HEAL_RATIO * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
    }

    [ClientRpc]
    public void RpcPlayTakeDamage()
    {
        if(isLocalPlayer)
        {
            GameObject.Find("UI").GetComponent<SoundManager>().PlayTakeDamage();
        }
    }

    [ClientRpc]
    public void RpcPlayEnemyDeath()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("UI").GetComponent<SoundManager>().PlayEnemyDeath();
        }
    }

    [ClientRpc]
    public void RpcPlayBuildTower()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("UI").GetComponent<SoundManager>().PlayBuildTower();
        }
    }

    [Command]
    public void CmdAttack(GameObject target)
    {
        var damage = Constants.HERO_BASE_ATTACK_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, damage);
    }

    [Command]
    public void CmdCastAbility(GameObject target)
    {
        var damage = Constants.HERO_BASE_ABILITY_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().CastAbility(gameObject, target, damage);
    }

    [Command]
    public void CmdBuildTower(GameObject slot)
    {
        if (GameObject.Find("ServerObject").GetComponent<TowersManager>().BuildTower(slot, tag == "Human"))
        {
            RpcPlayBuildTower();
        }
    }
}
