using UnityEngine;
using UnityEngine.Networking;

public class TowerBehaviour : LifeBehaviour {

    public GameObject slot;

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
        var manager = GameObject.Find("ServerObject").GetComponent<TowersManager>();
        // The tower is not on front line
        if(!manager.IsTowerVulnable(slot.name, tag == "Human"))
        {
            return;
        }
        base.TakeDamage(Damage);
        if (IsDead())
        {
            manager.TowerDestroyed(slot.name);
            NetworkServer.Destroy(gameObject);
        }
    }
}
