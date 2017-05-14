using UnityEngine;
using UnityEngine.Networking;

public class MinionBehaviour : NetworkBehaviour {

    public GameObject groupLeader;
    public int groupId;
    /*
    * Lane 1 is on the left for humans and on the right for aliens
    * Lane 2 is on the center
    * Lane 3 is on the right for humans and on the left for aliens
    */
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
