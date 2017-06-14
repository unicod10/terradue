using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrimeArenaMobSpawn : NetworkBehaviour {


    public GameObject monsterPrefab;
    private float elapsedTime;
    private bool spawned;
	// Use this for initialization
	void Start () {
        if(!isServer)
        {
            return;
        }
        //At the beginning there'll be no monster spawned
        elapsedTime = 0.0f;
        spawned = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime>=Constants.PRIME_SPAWN_TIME_SECONDS)
        {
            if (!spawned)
            {
                // Spawn the monster in prime arena
                var monster = Instantiate(monsterPrefab, transform.position, Quaternion.AngleAxis(90, new Vector3(0, 1)));
                monster.GetComponent<PrimeArenaMobBehaviour>().arena = gameObject;
                NetworkServer.Spawn(monster);
                spawned = true;
                //Reset the counter time
                elapsedTime = 0.0f;
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("PrimeMonster") != null)
                {
                    // Check again in one second
                    elapsedTime = Constants.PRIME_SPAWN_TIME_SECONDS - 1;
                }
                else
                {
                    spawned = false;
                    elapsedTime = Constants.PRIME_SPAWN_TIME_SECONDS - 15;
                }
            }
        }
    }
}
