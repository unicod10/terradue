using UnityEngine;
using UnityEngine.Networking;

public class LifeBehaviour : NetworkBehaviour {

    [SyncVar]
    protected float Health;
    [SyncVar]
    protected float MaximumHealth;
    protected float HealRatio;
    private float NotHitSince;
    private float HealState;

    protected LifeBehaviour(float MaximumHealth, float HealRatio)
    {
        this.MaximumHealth = MaximumHealth;
        this.HealRatio = HealRatio;
    }
    
	protected virtual void Start () {
		if(!isServer)
        {
            return;
        }
        Health = MaximumHealth;
        RpcUpdateHealth(Health, Health);
        NotHitSince = 0;
        HealState = 0;
    }
	
	protected virtual void Update ()
    {
        if (!isServer)
        {
            return;
        }
        NotHitSince += Time.deltaTime;
        // Start healing after
        if (HealState == 0 && NotHitSince >= Constants.HEAL_AFTER)
        {
            HealState = 1;
            NotHitSince = 1;
        }
        // Heal every second
        else if(HealState == 1 && NotHitSince >= 1)
        {
            if (GetComponent<PlayerBehaviour>() != null && Health < MaximumHealth && Health > 0)
            {
                HealRatio = GetComponent<PlayerBehaviour>().CalcHealRatio();
            }
            if (Health < MaximumHealth && Health > 0)
            {
                Health = Mathf.Min(Health + HealRatio, MaximumHealth);
                RpcUpdateHealth(Health, MaximumHealth);
                NotHitSince = 0;
            }
        }
    }

    virtual public float TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return 0;
        }
        Health = Mathf.Max(0, Health - Damage);
        RpcUpdateHealth(Health, MaximumHealth);
        NotHitSince = 0;
        HealState = 0;
        return 0;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public bool IsDead()
    {
        return !IsAlive();
    }

    public float GetHealth()
    {
        return Health;
    }

    [ClientRpc]
    protected void RpcUpdateHealth(float NewHealth, float newMaximumHealth)
    {
        Health = NewHealth;
        MaximumHealth = newMaximumHealth;
        if (!isLocalPlayer)
        {
            transform.Find("HealthBar/Bar").transform.localScale = new Vector3(NewHealth / MaximumHealth, 1f, 1f);
        }
        UpdateHealth(NewHealth, newMaximumHealth);
    }

    virtual protected void UpdateHealth(float NewHealth, float newMaximumHealth) {
    }
}
