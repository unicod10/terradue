using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HumanMinionAnimations : NetworkBehaviour,IAnimations {

	public void PlayIdle()
	{
		if (isServer)
		{
			RpcPlayIdle();
		}
	}

	[ClientRpc]
	private void RpcPlayIdle()
	{
		GetComponent<Animator>().Play("demo_combat_idle");
	}

	public void PlayMoving()
	{
		if(isServer)
		{
			RpcPlayMoving();
		}
	}

	[ClientRpc]
	private void RpcPlayMoving()
	{
		GetComponent<Animator>().Play("demo_combat_run");
	}

	public void PlayAttacking()
	{
		if (isServer)
		{
			RpcPlayAttacking();
		}
	}

	[ClientRpc]
	private void RpcPlayAttacking()
	{
		GetComponent<Animator>().Play("demo_combat_shoot", -1, 0f);
	}
}
