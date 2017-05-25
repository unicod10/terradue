using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : LifeBehaviour {
    
    public int groupId;

    public MinionBehaviour() : base(Constants.MINION_HEALTH, 0) {
    }

    protected override void Start()
    {
        base.Start();
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
