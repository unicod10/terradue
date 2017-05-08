using UnityEngine;
using UnityEngine.Networking;

public class HealthScript : NetworkBehaviour {

    public int BaseHealth;

    [SyncVar(hook = "UpdateHealth")]
    private int Health;

    public void Start()
    {
        if(!isServer)
        {
            return;
        }
        Health = BaseHealth;
    }
    public void TakeDamage(int Damage)
    {
        if(!isServer)
        {
            return;
        }
        Health -= Damage;
    }

    private void UpdateHealth(int NewHealth)
    {
        transform.Find("HealthBar").transform.Find("Bar").transform.localScale = new Vector3((NewHealth / 100f), 1f, 1f);
    }
}
