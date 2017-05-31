using UnityEngine;
using UnityEngine.Networking;

public class OptimusArenaSpawn : NetworkBehaviour {
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
	//Optimus monster will be spawned in this way: every five minutes it verifies if heroes level of both teams are balanced.
	//If they are balanced over five minutes, it will be spawned
	void Update () {
		if (!isServer)
		{
			return;
		}
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= Constants.OPTIMUS_SPAWN_TIME_SECONDS)
        {
            int humanTotalLevel = 0;
            int alienTotalLevel = 0;
            for(int i = 1; i <= 3; i++)
            {
                var humanHero = GameObject.Find("HumanHero" + i);
                var alienHero = GameObject.Find("AlienHero" + i);
                if(humanHero != null)
                {
                    humanTotalLevel += humanHero.GetComponent<PlayerBehaviour>().GetLevel();
                }
                if(alienHero != null)
                {
                    alienTotalLevel += alienHero.GetComponent<PlayerBehaviour>().GetLevel();
                }
            }
            // TODO check if still alive then spawned = true
            spawned = false;
            //if the difference is littler than 5 over 5 minutes of play, i'll spawn it
            if (Mathf.Abs(humanTotalLevel - alienTotalLevel) < Constants.OPTIMUS_SPAWN_ON_LEVEL_DIFF)
            {
                // TODO
            }
            elapsedTime = 0;
        }
	}
}
