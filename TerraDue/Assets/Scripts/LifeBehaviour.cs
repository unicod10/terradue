using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LifeBehaviour : NetworkBehaviour {

    protected float MaximumHealth;
    protected float HealRatio;

    [SyncVar(hook = "PrivateUpdateHealth")]
    protected float Health;

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
	}
	
	protected virtual void Update ()
    {
        if (!isServer)
        {
            return;
        }
        // TODO heal
    }

    virtual public void TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return;
        }
        Health = Mathf.Max(0, Health - Damage);
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public bool IsDead()
    {
        return !IsAlive();
    }

    private void PrivateUpdateHealth(float NewHealth)
    {
        transform.Find("HealthBar/Bar").transform.localScale = new Vector3(NewHealth / MaximumHealth, 1f, 1f);
        UpdateHealth(NewHealth);
    }

    virtual protected void UpdateHealth(float NewHealth) {
    }
}
