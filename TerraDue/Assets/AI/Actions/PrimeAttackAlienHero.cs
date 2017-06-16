using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PrimeAttackAlienHero : RAINAction
{
	GameObject target;
	PrimeArenaMobBehaviour action;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
		target = ai.WorkingMemory.GetItem<GameObject>("aCloseAlienHero");
		action = ai.Body.GetComponentInChildren<PrimeArenaMobBehaviour>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		action.Attack(target);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}