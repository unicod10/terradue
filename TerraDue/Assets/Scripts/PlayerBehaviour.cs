using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerBehaviour : LifeBehaviour
{
    private float deadSince;
    private float experience;
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
            GameObject.Find("UI").GetComponent<UserInteraction>().player = gameObject;
            transform.Find("HealthBar").transform.position = new Vector3(1000, 1000, 1000);
			SoundManager.instance.playSoundEffect (spawnSound);
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
		SoundManager.instance.playSoundEffect (damageSound);
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
            GameObject.Find("UI").GetComponent<UserInteraction>().SetDefaultMessage();
			SoundManager.instance.playSoundEffect (spawnSound);
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

    public int GetLevel()
    {
        int level = 0;
        float calcExperience = experience;
        do
        {
            level++;
            float neededExperience = Constants.HERO_BASE_LEVELUP_EXP * Mathf.Pow(1 + Constants.BALANCING_INTEREST, level - 1);
            calcExperience -= neededExperience;
			SoundManager.instance.playSoundEffect(levelUpSound);
        }
        while (calcExperience >= 0);
        return level;
    }

    public float CalcHealRatio()
    {
        return Constants.HERO_BASE_HEAL_RATIO * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
    }

    [Command]
    public void CmdAttack(GameObject target)
    {
        var damage = Constants.HERO_BASE_ATTACK_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, damage);
		SoundManager.instance.playSoundEffect(attackSound);
    }

    [Command]
    public void CmdCastAbility(GameObject target)
    {
        var damage = Constants.HERO_BASE_ABILITY_DAMAGE * Mathf.Pow(1 + Constants.BALANCING_INTEREST, GetLevel() - 1);
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().CastAbility(gameObject, target, damage);
		SoundManager.instance.playSoundEffect(castAbilitySound);
    }

    [Command]
    public void CmdBuildTower(GameObject slot)
    {
        GameObject.Find("ServerObject").GetComponent<TowersManager>().BuildTower(slot, tag == "Human");
		SoundManager.instance.playSoundEffect(buildTowerSound);
    }
}
