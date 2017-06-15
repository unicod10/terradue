using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JungleMobAnimations : NetworkBehaviour, IAnimations {
    
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
        GetComponent<Animator>().Play("Insect_Idle");
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
        GetComponent<Animator>().Play("Insect_Walk");
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
        GetComponent<Animator>().Play("Insect_Attack", -1, 0f);
    }
}
