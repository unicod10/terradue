﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class PrimeArenaMobBehaviour : LifeBehaviour {
    
    public PrimeArenaMobBehaviour() : base(Constants.PRIME_HEALTH, 0) {
        
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
            return Constants.PRIME_ARENA_EXPERIENCE;
        }
        return 0;
    }
}
