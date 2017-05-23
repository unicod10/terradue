using UnityEngine;
using UnityEngine.Networking;

public class BaseBehaviour : LifeBehaviour {
    
    public BaseBehaviour() : base(Constants.BASE_HEALTH, 0) {
    }

	protected override void Start () {
        base.Start();	
	}
	
	protected override void Update () {
        base.Update();
	}

    public override void TakeDamage(float Damage)
    {
        if(!isServer)
        {
            return;
        }
        var manager = GameObject.Find("ServerObject").GetComponent<TowersManager>();
        // The are still functioning towers
        if (!manager.IsBaseVunlnable(tag == "Human"))
        {
            return;
        }
        base.TakeDamage(Damage);
        if(IsDead())
        {
            RpcEndGame();
        }
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Debug.Log("End game");
    }
}
