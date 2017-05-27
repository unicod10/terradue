using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class JungleMobSpawnBehaviour : NetworkBehaviour {

    public GameObject JungleMobPrefab;
    

 
    
    

    void Start()
    {
        if (!isServer)
        {
            return;
        }



        //We spawn a group of 4 mob one time per all match

        var mob = Instantiate(JungleMobPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(mob);
        NetworkServer.Spawn(mob);
        NetworkServer.Spawn(mob);
        NetworkServer.Spawn(mob);
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

}
