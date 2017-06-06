using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AlienHeroAnimations : NetworkBehaviour, IAnimations {

    public void PlayIdle()
    {
        if (isLocalPlayer)
        {
            CmdPlayIdle();
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
        // TODO
    }

    public void PlayMoving()
    {
        if (isLocalPlayer)
        {
            CmdPlayMoving();
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
        // TODO
    }

    public void PlayAttacking()
    {
        if (isLocalPlayer)
        {
            CmdPlayAttacking();
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
        // TODO
    }
}
