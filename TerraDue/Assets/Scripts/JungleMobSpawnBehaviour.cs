﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class JungleMobSpawnBehaviour : NetworkBehaviour
{
    public GameObject JungleMobPrefab;
    void Start()
    {
        if (!isServer)
        {
            return;
        }

        //We spawn a group of 4 mob one time per all match

        var mob = Instantiate(JungleMobPrefab, transform.position + new Vector3(3f, 0.2f), Quaternion.AngleAxis(90, new Vector3(0, 1)));
        NetworkServer.Spawn(mob);
        mob = Instantiate(JungleMobPrefab, transform.position + new Vector3(-3f, 0.2f), Quaternion.AngleAxis(90, new Vector3(0, 1)));
        NetworkServer.Spawn(mob);
        mob = Instantiate(JungleMobPrefab, transform.position + new Vector3(0f, 0.2f, -3f), Quaternion.AngleAxis(90, new Vector3(0, 1)));
        NetworkServer.Spawn(mob);
        mob = Instantiate(JungleMobPrefab, transform.position + new Vector3(0f, 0.2f, 3f), Quaternion.AngleAxis(90, new Vector3(0, 1)));
        NetworkServer.Spawn(mob);
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }
    }
}
