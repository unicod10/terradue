using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionClass : MonoBehaviour {
    /*
        lane e team sono due parametri per stabilire sempre a quale squadra e quale lane appartiene il minion in questione.
        lane: 1-Destra 2-Centro 3-Sinistra
        team: 1-Umani 2-Alieni
        */
    private int lane;
    private int team;
    private int lifePoints;
    private int atkPower;
    private float criticalRate;
    private Vector3 position;

	// Use this for initialization
	void Start () {
       
        this.atkPower = 10;
        this.criticalRate = 0.05f;
        this.lifePoints = 100;
	}
	void setInitialparam(int laneToAssign, int teamToAssign, Vector3 positionToAssign)
    {
        this.lane = laneToAssign;
        this.team = teamToAssign;
        this.position = positionToAssign;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
