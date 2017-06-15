﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AlienMinionAnimations : NetworkBehaviour, IAnimations {

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
		GetComponent<Animator>().Play("alienm_idle");
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
		GetComponent<Animator>().Play("alienm_moving");
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
		GetComponent<Animator>().Play("alienm_shot", -1, 0f);
	}
}
