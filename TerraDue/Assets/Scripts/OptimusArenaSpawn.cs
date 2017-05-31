using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimusArenaSpawn : MonoBehaviour {
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
		if (elapsedTime >= 300.0f) {
			//I'll take all players level by them gameobject 
			//if the difference is littler than 5 over 5 minutes of play, i'll spawn it
			//int humanTotalLevel=GameObject.Find("HumanHero1").GetComponent("PlayerBehaviour").
		}
	}
}
