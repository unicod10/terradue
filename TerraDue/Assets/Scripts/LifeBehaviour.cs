using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LifeBehaviour : NetworkBehaviour {
    
    [SyncVar]
    protected float MaximumHealth;
    [SyncVar]
    protected float Health;
    protected float HealRatio;

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
    }
	
	protected virtual void Update ()
    {
        if (!isServer)
        {
            return;
        }
        // TODO heal
    }

    virtual public float TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return 0;
        }
        Health = Mathf.Max(0, Health - Damage);
        RpcUpdateHealth(Health, MaximumHealth);
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
