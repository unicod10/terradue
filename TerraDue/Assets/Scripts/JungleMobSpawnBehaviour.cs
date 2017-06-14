using System.Collections;
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
        // We spawn a group of 4 mob one time per all match
        SpawnJungleMob(new Vector3(+3f, 0.2f, +0f));
        SpawnJungleMob(new Vector3(-3f, 0.2f, +0f));
        SpawnJungleMob(new Vector3(+0f, 0.2f, -3f));
        SpawnJungleMob(new Vector3(+0f, 0.2f, +3f));
    }

    private void SpawnJungleMob(Vector3 positionOffset)
    {
        var mob = Instantiate(JungleMobPrefab, transform.position + positionOffset, Quaternion.AngleAxis(90, new Vector3(0, 1)));
        mob.GetComponent<JungleMobBehaviour>().jungle = gameObject;
        mob.GetComponent<JungleMobBehaviour>().positionOffset = positionOffset;
        NetworkServer.Spawn(mob);
    }
}
