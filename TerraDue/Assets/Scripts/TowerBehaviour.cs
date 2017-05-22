using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerBehaviour : LifeBehaviour {

    public TowerBehaviour() : base(Constants.TOWER_HEALTH, 0) {
    }
    
	protected override void Start () {
        base.Start();	
	}
	
	protected override void Update () {
        base.Update();	
	}
    
    public override void TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return;
        }
        // TODO check if vulnable
        base.TakeDamage(Damage);
        if (IsDead())
        {
            // TODO free slot
            NetworkServer.Destroy(gameObject);
        }
    }
}
