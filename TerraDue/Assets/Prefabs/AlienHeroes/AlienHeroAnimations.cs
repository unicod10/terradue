using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AlienHeroAnimations : NetworkBehaviour, IAnimations {

    private bool alreadyAttacking;

    public void Start()
    {
        if(isLocalPlayer)
        {
            alreadyAttacking = false;
        }
    }

    public void PlayIdle()
    {
        if (isLocalPlayer)
        {
            CmdPlayIdle();
            alreadyAttacking = false;
        }
    }

    [Command]
    public void CmdPlayIdle()
    {
        RpcPlayIdle();
    }

    [ClientRpc]
    private void RpcPlayIdle()
    {
        GetComponent<Animation>().Play("Idle");
    }

    public void PlayMoving()
    {
        if (isLocalPlayer)
        {
            CmdPlayMoving();
            alreadyAttacking = false;
        }
    }

    [Command]
    public void CmdPlayMoving()
    {
        RpcPlayMoving();
    }

    [ClientRpc]
    private void RpcPlayMoving()
    {
        GetComponent<Animation>().Play("Moving");
    }

    public void PlayAttacking()
    {
        if (isLocalPlayer)
        {
            CmdPlayAttacking();
            alreadyAttacking = true;
        }
    }

    [Command]
    public void CmdPlayAttacking()
    {
        RpcPlayAttacking();
    }

    [ClientRpc]
    private void RpcPlayAttacking()
    {
        GetComponent<Animation>().Play("Attacking");
    }
}
