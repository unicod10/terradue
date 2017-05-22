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
        // TODO check if vulnable
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
