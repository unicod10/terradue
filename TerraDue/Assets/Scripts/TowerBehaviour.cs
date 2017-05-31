using UnityEngine;
using UnityEngine.Networking;

public class TowerBehaviour : LifeBehaviour {

    public GameObject slot;

    public TowerBehaviour() : base(Constants.TOWER_HEALTH, Constants.TOWER_HEAL_RATIO) {
    }
    
	protected override void Start () {
        base.Start();	
	}
	
	protected override void Update () {
        base.Update();	
	}
    
    public override float TakeDamage(float Damage)
    {
        if (!isServer)
        {
            return 0;
        }
        var manager = GameObject.Find("ServerObject").GetComponent<TowersManager>();
        // The tower is not on front line
        if(!manager.IsTowerVulnable(slot.name, tag == "Human"))
        {
            return 0;
        }
        base.TakeDamage(Damage);
        if (IsDead())
        {
            manager.TowerDestroyed(slot.name);
            NetworkServer.Destroy(gameObject);
            return Constants.TOWER_EXPERIENCE;
        }
        return 0;
    }

    public void Attack(GameObject target)
    {
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, Constants.TOWER_ATTACK_DAMAGE);
    }
}
