using UnityEngine;
using UnityEngine.Networking;

public class HumanHeroAnimations : NetworkBehaviour, IAnimations {
    
    public void PlayIdle()
    {
        if(isLocalPlayer)
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
        GetComponent<Animator>().Play("assault_combat_idle");
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
        GetComponent<Animator>().Play("assault_combat_run");
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
        GetComponent<Animator>().Play("assault_combat_shoot", -1, 0f);
    }
}
