using UnityEngine;
using UnityEngine.Networking;

public class MinionSpawnBehaviour : NetworkBehaviour {


    public GameObject minionLeaderPrefab;
    public GameObject minionSoldierPrefab;
    public bool isAlien;

    private enum State
    {
        Waiting, Spawning
    }
    private State state;
    private float elapsedTime;
    private int spawnedMembers;
	private GameObject groupLeader;

    void Start ()
    {
        if (!isServer)
        {
            return;
        }
        // Spawn the first group after
        elapsedTime = Constants.MINION_NEXT_GROUP_AFTER - Constants.MINION_SPAWN_FIRST_AFTER;
        state = State.Waiting;
        spawnedMembers = 0;
	}
	
	void Update ()
    {
        if (!isServer)
        {
            return;
        }
        elapsedTime += Time.deltaTime;

        // Time to spawn a new group
        if (state == State.Waiting && elapsedTime >= Constants.MINION_NEXT_GROUP_AFTER)
        {
            state = State.Spawning;
            elapsedTime = 0;
            spawnedMembers++;

            // Spawn the group leader
            var minion = Instantiate(minionLeaderPrefab, transform.position, Quaternion.identity);
			minion.GetComponent<MinionBehaviour> ().groupLeader = minion;
			groupLeader = minion;
            if(isAlien)
            {
                minion.transform.Rotate(new Vector3(0, 1, 0), 180);
            }
            NetworkServer.Spawn(minion);
        }

        // Time to spawn new followers
        else if(state == State.Spawning && elapsedTime >= Constants.MINION_NEXT_MEMBER_AFTER)
        {
            spawnedMembers++;
            elapsedTime = 0;

            // Spawn the soldiers
            var minion = Instantiate(minionSoldierPrefab, transform.position, Quaternion.identity);
			minion.GetComponent<MinionBehaviour> ().groupLeader = groupLeader;
            if (isAlien)
            {
                minion.transform.Rotate(new Vector3(0, 1, 0), 180);
            }
            NetworkServer.Spawn(minion);

            // Go back to sleep
            if (spawnedMembers == Constants.MINION_GROUP_SIZE)
            {
                state = State.Waiting;
                spawnedMembers = 0;
            }
        }
    }
}