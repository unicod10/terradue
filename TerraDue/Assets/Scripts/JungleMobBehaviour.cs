using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class JungleMobBehaviour : LifeBehaviour {

    public int groupId;

    public JungleMobBehaviour() : base(Constants.JUNGLE_MOB_LIFE, 0) {
    }

    protected override void Start()
    {
        base.Start();
    }

    public override float TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (isServer && IsDead())
        {
            NetworkServer.Destroy(gameObject);
            return Constants.JUNGLE_MOB_EXPERIENCE;
        }
        return 0;
    }
}
