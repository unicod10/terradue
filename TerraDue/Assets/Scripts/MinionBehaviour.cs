using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : NetworkBehaviour {

    public GameObject groupLeader;
    public int groupId;
    public int lane;

    void Update ()
    {
		if(!isServer)
        {
            return;
        }
	}

    public bool IsGroupLeader()
    {
        return groupLeader == this;
    }

    public bool HasLeader()
    {
        return groupLeader != null;
    }
}
