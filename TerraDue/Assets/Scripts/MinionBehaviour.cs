using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : LifeBehaviour {
    
    public int groupId;
	public GameObject groupLeader;
	 
    public MinionBehaviour() : base(Constants.MINION_HEALTH, 0) {
    }

    protected override void Start()
    {
        base.Start();
        if(isServer)
        {
            GetComponent<RAIN.Entities.EntityRig>().enabled = true;
            GetComponent<RAIN.Core.AIRig>().enabled = true;
        }
        else if(isClient)
        {
            GetComponent<RAIN.Entities.EntityRig>().enabled = false;
            GetComponent<RAIN.Core.AIRig>().enabled = false;
        }
		if (isServer)
		{
			GetComponent<IAnimations> ().PlayIdle();
		}
    }

    public override float TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (isServer && IsDead())
        {
            NetworkServer.Destroy(gameObject);
            return Constants.MINION_EXPERIENCE;
        }
        return 0;
    }

	public void Attack(GameObject target)
	{
		GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, Constants.MINION_ATTACK_DAMAGE);
	}
}
