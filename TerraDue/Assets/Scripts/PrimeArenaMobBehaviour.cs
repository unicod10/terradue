using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            GetComponent<RAIN.Core.AIRig>().enabled = true;
            if (GetComponent<RAIN.Entities.EntityRig>())
            {
                GetComponent<RAIN.Entities.EntityRig>().enabled = true;
            }
            //Setto la posizione a lui assegnata nella memoria dell'AI
            GetComponentInChildren<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem<GameObject>("myBush", arena);
        }
        else if (isClient)
        {
            GetComponent<RAIN.Core.AIRig>().enabled = false;
            if (GetComponent<RAIN.Entities.EntityRig>())
            {
                GetComponent<RAIN.Entities.EntityRig>().enabled = false;
            }
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
