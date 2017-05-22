using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : LifeBehaviour {

    public GameObject groupLeader;
    public int groupId;
    /*
    * Lane 0 is on the left for humans and on the right for aliens
    * Lane 1 is on the center
    * Lane 2 is on the right for humans and on the left for aliens
    */
    public int lane;

    public MinionBehaviour() : base(Constants.MINION_HEALTH, 0) {
    }

    protected override void Start()
    {
        base.Start();
    }
    
    public bool IsGroupLeader()
    {
        return groupLeader == gameObject;
    }

    public bool HasLeader()
    {
        return groupLeader != null;
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (isServer && IsDead())
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
