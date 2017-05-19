using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : NetworkBehaviour, ITakeDamage {

    public GameObject groupLeader;
    public int groupId;
    /*
    * Lane 1 is on the left for humans and on the right for aliens
    * Lane 2 is on the center
    * Lane 3 is on the right for humans and on the left for aliens
    */
    public int lane;
    [SyncVar(hook = "UpdateHealth")]
    public float Health;

    private void Start()
    {
        if(!isServer)
        {
            return;
        }
        Health = Constants.MINION_HEALTH;
    }
    
    public bool IsGroupLeader()
    {
        return groupLeader == gameObject;
    }

    public bool HasLeader()
    {
        return groupLeader != null;
    }

    public void TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return;
        }
        Health -= Damage;
        if(Health <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private void UpdateHealth(float NewHealth)
    {
        transform.Find("HealthBar/Bar").transform.localScale = new Vector3(NewHealth / Constants.MINION_HEALTH, 1f, 1f);
    }
}
