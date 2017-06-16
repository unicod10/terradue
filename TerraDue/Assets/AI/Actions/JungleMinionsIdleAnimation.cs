using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class JungleMinionsIdleAnimation : RAINAction
{
	JungleMobAnimations action;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
		action = ai.Body.GetComponentInChildren<JungleMobAnimations>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		action.PlayIdle ();
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}