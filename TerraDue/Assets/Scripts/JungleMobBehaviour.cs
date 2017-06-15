using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RAIN.Core;

public class JungleMobBehaviour : LifeBehaviour {
    
    public GameObject jungle;
    public Vector3 positionOffset;
	private GameObject dummyPosition;

    public JungleMobBehaviour() : base(Constants.JUNGLE_MOB_LIFE, 0) {
    }

	protected override void Start()
    {
        base.Start();
        if (isServer)
        {
            GetComponent<IAnimations>().PlayIdle();
            GetComponent<RAIN.Core.AIRig>().enabled = true;
            if (GetComponent<RAIN.Entities.EntityRig>())
            {
                GetComponent<RAIN.Entities.EntityRig>().enabled = true;
            }
            //Setto la posizione a lui assegnata nella memoria dell'AI
            dummyPosition = new GameObject("DummyObj");
            dummyPosition.transform.position += jungle.transform.position + positionOffset;
            GetComponentInChildren<AIRig>().AI.WorkingMemory.SetItem<GameObject>("myBush", dummyPosition);
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
            return Constants.JUNGLE_MOB_EXPERIENCE;
        }
        return 0;
    }

    public void Attack(GameObject target)
    {
        GameObject.Find("ServerObject").GetComponent<AttacksManager>().Attack(gameObject, target, Constants.JUNGLE_MOB_ATTACK_DAMAGE);
    }
}
