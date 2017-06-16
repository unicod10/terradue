using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class JungleMinionAttackHumanHero : RAINAction
{
	GameObject target;
	JungleMobBehaviour action;
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
		target = ai.WorkingMemory.GetItem<GameObject>("aCloseHumanHero");
		action = ai.Body.GetComponentInChildren<JungleMobBehaviour>();

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