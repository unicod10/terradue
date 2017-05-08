using UnityEngine;
using UnityEngine.Networking;

public class MinionSpawnScrit : NetworkBehaviour {

    public int spawnEverySecs;
    public GameObject humanMinionPrefab;
    public GameObject alienMinionPrefab;
    private float elapsedTime;
    private int turn;
    private int nameIndex;

	void Start ()
    {
		if(isServer)
        {
            // Spawn the first two minions
            turn = 1;
            nameIndex = 0;
            spawnMinions(turn);
            elapsedTime = 0;
        }
	}
	
	void Update ()
    {
        if (isServer)
        {
            elapsedTime += Time.deltaTime;
            // Spawn periodically
            if (elapsedTime >= spawnEverySecs)
            {
                ++turn;
                if(turn == 4)
                {
                    turn = 1;
                }
                ++nameIndex;
                spawnMinions(turn);
                elapsedTime = 0;
                // TODO remove
                GameObject.Find("HumanHero1").GetComponent<HealthScript>().TakeDamage(20);
            }
        }
    }

    private void spawnMinions(int turn)
    {
        var spawnPointHuman = GameObject.Find("SpawnHuman" + turn).transform.position;
        var spawnPointAlien = GameObject.Find("SpawnAlien" + turn).transform.position;
        GameObject humanMinion = GameObject.Instantiate<GameObject>(this.humanMinionPrefab, spawnPointHuman, Quaternion.identity);
        GameObject alienMinion = GameObject.Instantiate<GameObject>(this.alienMinionPrefab, spawnPointAlien, Quaternion.identity);
        humanMinion.name = "HumanMinion_" + nameIndex;
        alienMinion.name = "AlienMinion_" + nameIndex;
        alienMinion.transform.Rotate(new Vector3(0, 1, 0), 180);
        NetworkServer.Spawn(humanMinion);
        NetworkServer.Spawn(alienMinion);
    }
}
