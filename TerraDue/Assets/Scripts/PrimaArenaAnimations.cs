using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrimaArenaAnimations : NetworkBehaviour, IAnimations {

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
        GetComponent<Animator>().Play("Idle");
    }

    public void PlayMoving()
    {
        if (isServer)
        {
            RpcPlayMoving();
        }
    }

    [ClientRpc]
    private void RpcPlayMoving()
    {
        GetComponent<Animator>().Play("Run");
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
        GetComponent<Animator>().Play("Attack", -1, 0f);
    }
}
