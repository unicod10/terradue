using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour, ITakeDamage {
    
    [SyncVar(hook = "UpdateHealth")]
    private float Health;
    [SyncVar]
    private float DeadSince;

    public void Start()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("UI").GetComponent<MouseClick>().player = gameObject;
        }
        if (isServer)
        {
            Health = Constants.HERO_BASE_HEALTH;
            DeadSince = -1;
        }
    }

    public void Update()
    {
        if(!isServer)
        {
            return;
        }
        // If dead check if time to respawn
        if(DeadSince >= 0)
        {
            DeadSince += Time.deltaTime;
            if(DeadSince >= Constants.RESPAWN_AFTER_SECS)
            {
                Health = GetFullHealth();
                DeadSince = -1;
                RpcRespawn();
            }
        }
    }

    public void TakeDamage(float Damage)
    {
        if(!isServer)
        {
            return;
        }
        Health = Mathf.Max(0, Health-Damage);
        if(Health == 0)
        {
            DeadSince = 0;
            RpcDie();
        }
    }

    // Callable from client too thanks to SyncVar
    public bool IsAlive()
    {
        return DeadSince < 0;
    }

    private float GetFullHealth()
    {
        return Constants.HERO_BASE_HEALTH;
    }

    private void UpdateHealth(float NewHealth)
    {
        // Update health bar
        var scale = new Vector3(NewHealth / GetFullHealth(), 1f, 1f);
        transform.Find("HealthBar/Bar").transform.localScale = scale;

        // Update UI
        if(isLocalPlayer)
        {
            GameObject.Find("UI/Panel/Health/Bar").transform.localScale = scale;
        }
    }

    [ClientRpc]
    private void RpcDie()
    {
        GetComponent<MoveToPoint>().Hide();
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if(isLocalPlayer)
        {
            GetComponent<MoveToPoint>().Spawn();
        }
    }

    [Command]
    public void CmdBuildTower()
    {
        // TODO
    }
}
