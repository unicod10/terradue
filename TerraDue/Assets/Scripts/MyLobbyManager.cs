﻿using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class MyLobbyManager : LobbyManager {

    private const int ALIEN_PREFAB_OFFSET = 3;
    private const int ABILITY_PREFAB_OFFSET = 8;
    private const int TOWER_PREFAB_OFFSET = 10;

    private int spawnedHumans = 0;
    private int spawnedAliens = 0;

    // https://forum.unity3d.com/threads/which-function-to-override.391076/#post-2571472
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform spawnPoint;
        GameObject heroPrefab;
        GameObject player;
        string name;
        Quaternion rotation;

        // Spawn at hero's unique position
        if (spawnedHumans == spawnedAliens)
        {
            var playerIndex = spawnedHumans++;
            spawnPoint = GameObject.Find("SpawnHuman" + playerIndex).transform;
            heroPrefab = spawnPrefabs[playerIndex];
            name = "HumanHero" + playerIndex;
            rotation = Quaternion.identity;
        }
        else
        {
            var playerIndex = spawnedAliens++;
            spawnPoint = GameObject.Find("SpawnAlien" + playerIndex).transform;
            heroPrefab = spawnPrefabs[playerIndex + ALIEN_PREFAB_OFFSET];
            name = "AlienHero" + playerIndex;
            rotation = Quaternion.AngleAxis(180, Vector3.up);
        }

        player = Instantiate(heroPrefab, spawnPoint.position, rotation);
        player.name = name;
        return player;
    }

    public GameObject GetAbilityPrefab(bool human)
    {
        return spawnPrefabs[human ? ABILITY_PREFAB_OFFSET : ABILITY_PREFAB_OFFSET + 1];
    }

    public GameObject GetTowerPrefab(bool human)
    {
        return spawnPrefabs[human ? TOWER_PREFAB_OFFSET : TOWER_PREFAB_OFFSET + 1];
    }
}