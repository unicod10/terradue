using UnityEngine;
using UnityEngine.Networking;

public class HealthBehaviour : NetworkBehaviour {

    public float BaseHealth;

    [SyncVar(hook = "UpdateHealth")]
    private float Health;

    public void Start()
    {
        if(!isServer)
        {
            return;
        }
        Health = BaseHealth;
    }

    public void TakeDamage(float Damage)
    {
        if(!isServer)
        {
            return;
        }
        Health -= Damage;
    }

    public float getHealth()
    {
        return Health;
    }

    private void UpdateHealth(float NewHealth)
    {
        transform.Find("HealthBar").transform.Find("Bar").transform.localScale = new Vector3((NewHealth / 100), 1f, 1f);
    }
}
