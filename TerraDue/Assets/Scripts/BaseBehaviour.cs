using UnityEngine;
using UnityEngine.Networking;

public class BaseBehaviour : LifeBehaviour {
    
    public BaseBehaviour() : base(Constants.BASE_HEALTH, Constants.BASE_HEAL_RATIO) {
    }

	protected override void Start () {
        base.Start();	
	}
	
	protected override void Update () {
        base.Update();
	}

    public override float TakeDamage(float Damage)
    {
        if(!isServer)
        {
            return 0;
        }
        var manager = GameObject.Find("ServerObject").GetComponent<TowersManager>();
        // The are still functioning towers
        if (!manager.IsBaseVulernable(tag == "Human"))
        {
            return 0;
        }
        base.TakeDamage(Damage);
        if(IsDead())
        {
            RpcEndGame();
        }
        return 0;
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Debug.Log("End game");
    }
}
