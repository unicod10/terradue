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
        
        if ((elapsedTime>=Constants.PRIME_SPAWN_TIME_SECONDS) && (spawned == false))
        {
            // Spawn the monster in prime arena
            var monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            Debug.Log("Prime monster has been spawned!");
            NetworkServer.Spawn(monster);
            spawned = true;
            //Reset the counter time
            elapsedTime = 0.0f;
        }
        if (spawned)
        {

            if (GameObject.FindGameObjectWithTag("PrimeMonster")!=null)
            {
                Debug.Log("The monster is still alive!");
            }
            else
            {
                spawned = false;
                elapsedTime = 0.0f;
            }
        }

    }

}
