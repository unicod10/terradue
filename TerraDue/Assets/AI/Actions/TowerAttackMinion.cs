using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class TowerAttackMinion : RAINAction
{
	GameObject target;
	TowerBehaviour action;
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
		target = ai.WorkingMemory.GetItem<GameObject>("aCloseMinion");
		action = ai.Body.GetComponentInChildren<TowerBehaviour>();

	}

	public override ActionResult Execute(RAIN.Core.AI ai)
	{

		//action.Attack(target);
		Debug.Log("TOWER ATTACK MINION!!!!");
		return ActionResult.SUCCESS;
	}

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}