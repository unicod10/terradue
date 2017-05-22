using UnityEngine;
using UnityEngine.Networking;

public class MinionSpawnBehaviour : NetworkBehaviour {

    public int minionGroupSize;
    public int groupSpawnIntervalSecs;
    public int groupMemberSpawnIntervalSecs;
    public int spawnFirstGroupAfterSecs;
    public GameObject humanMinionPrefab;
    public GameObject alienMinionPrefab;

    private const int WAITING = 0;
    private const int SPAWNING = 1;
    private int state;
    private float elapsedTime;
    private int spawnedMembers;
    private int groupId;
    private int minionId;
    private GameObject[] groupLeaders = new GameObject[6];

    void Start ()
    {
        if (!isServer)
        {
            return;
        }
        // Spawn the first group after
        elapsedTime = groupSpawnIntervalSecs - spawnFirstGroupAfterSecs;
        state = WAITING;
        spawnedMembers = 0;
        groupId = 0;
        minionId = 0;
	}
	
	void Update ()
    {
        if (!isServer)
        {
            return;
        }
        elapsedTime += Time.deltaTime;

        // Time to spawn a new group
        if (state == WAITING && elapsedTime >= groupSpawnIntervalSecs)
        {
            state = SPAWNING;
            elapsedTime = 0;
            spawnedMembers++;

            // Spawn the six group leaders
            groupLeaders[0] = SpawnHumanLeader(groupId + 0, "SpawnHuman1", 0);
            groupLeaders[1] = SpawnHumanLeader(groupId + 1, "SpawnHuman0", 1);
            groupLeaders[2] = SpawnHumanLeader(groupId + 2, "SpawnHuman2", 2);
            groupLeaders[3] = SpawnAlienLeader(groupId + 3, "SpawnAlien2", 0);
            groupLeaders[4] = SpawnAlienLeader(groupId + 4, "SpawnAlien0", 1);
            groupLeaders[5] = SpawnAlienLeader(groupId + 5, "SpawnAlien1", 2);
        }

        // Time to spawn new followers
        else if(state == SPAWNING && elapsedTime >= groupMemberSpawnIntervalSecs)
        {
            spawnedMembers++;
            elapsedTime = 0;

            // Spawn one minion for each group
            SpawnHumanFollower(groupLeaders[0], groupId + 0, "SpawnHuman1", 0);
            SpawnHumanFollower(groupLeaders[1], groupId + 1, "SpawnHuman0", 1);
            SpawnHumanFollower(groupLeaders[2], groupId + 2, "SpawnHuman2", 2);
            SpawnAlienFollower(groupLeaders[3], groupId + 3, "SpawnAlien2", 0);
            SpawnAlienFollower(groupLeaders[4], groupId + 4, "SpawnAlien0", 1);
            SpawnAlienFollower(groupLeaders[5], groupId + 5, "SpawnAlien1", 2);

            // Go back to sleep
            if (spawnedMembers == minionGroupSize)
            {
                state = WAITING;
                spawnedMembers = 0;
                groupId += 6;
            }
        }
    }

    private GameObject SpawnHumanLeader(int groupId, string spawnPoint, int lane)
    {
        var minion = InstantiateHumanMinion(spawnPoint);
        FillAndSpawnMinion(minion, minion, groupId, lane);
        return minion;
    }

    private GameObject SpawnAlienLeader(int groupId, string spawnPoint, int lane)
    {
        var minion = InstantiateAlienMinion(spawnPoint);
        FillAndSpawnMinion(minion, minion, groupId, lane);
        return minion;
    }

    private void SpawnHumanFollower(GameObject groupLeader, int groupId, string spawnPoint, int lane)
    {
        FillAndSpawnMinion(InstantiateHumanMinion(spawnPoint), groupLeader, groupId, lane);
    }

    private void SpawnAlienFollower(GameObject groupLeader, int groupId, string spawnPoint, int lane)
    {
        FillAndSpawnMinion(InstantiateAlienMinion(spawnPoint), groupLeader, groupId, lane);
    }

    private GameObject InstantiateHumanMinion(string spawnPoint)
    {
        return GameObject.Instantiate<GameObject>(humanMinionPrefab, GetSpawnPosition(spawnPoint), Quaternion.identity);
    }

    private GameObject InstantiateAlienMinion(string spawnPoint)
    {
        var minion = GameObject.Instantiate<GameObject>(alienMinionPrefab, GetSpawnPosition(spawnPoint), Quaternion.identity);
        minion.transform.Rotate(new Vector3(0, 1, 0), 180);
        return minion;
    }

    private Vector3 GetSpawnPosition(string spawnName)
    {
        return GameObject.Find(spawnName).transform.position;
    }

    private void FillAndSpawnMinion(GameObject minion, GameObject groupLeader, int groupId, int lane)
    {
        var behaviour = minion.GetComponent<MinionBehaviour>();
        behaviour.groupLeader = groupLeader;
        behaviour.groupId = groupId;
        behaviour.lane = lane;
        NetworkServer.Spawn(minion);
    }
}
