using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Core;
using UnityEngine.Networking;

public class PrimeArenaMobBehaviour : LifeBehaviour {

    public GameObject arena;
    
    public PrimeArenaMobBehaviour() : base(Constants.PRIME_HEALTH, Constants.PRIME_HEAL_RATIO) {
    }

    protected override void Start()
    {
        base.Start();
        if(isServer)
        {
            GetComponent<IAnimations>().PlayIdle();
			//Setto la posizione a lui assegnata nella memoria dell'AI
			GetComponentInChildren<AIRig>().AI.WorkingMemory.SetItem<GameObject>("myBush",arena);
        }
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

    public void Attack(GameObject target)
    {
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, Constants.PRIME_ATTACK);
    }
}
