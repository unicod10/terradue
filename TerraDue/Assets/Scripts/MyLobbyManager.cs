using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class MyLobbyManager : LobbyManager {

    private const int ALIEN_PREFAB_OFFSET = 3;

    private int spawnedHumans = 0;
    private int spawnedAliens = 0;

    // https://forum.unity3d.com/threads/which-function-to-override.391076/#post-2571472
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform spawnPoint;
        GameObject heroPrefab;
        GameObject player;

        // Spawn at hero's unique position
        if (spawnedHumans == spawnedAliens)
        {
            var playerIndex = ++spawnedHumans;
            spawnPoint = GameObject.Find("SpawnHuman" + playerIndex).transform;
            heroPrefab = spawnPrefabs[playerIndex - 1];
        }
        else
        {
            var playerIndex = ++spawnedAliens;
            spawnPoint = GameObject.Find("SpawnAlien" + playerIndex).transform;
            heroPrefab = spawnPrefabs[playerIndex - 1 + ALIEN_PREFAB_OFFSET];
        }

        player = Instantiate(heroPrefab, spawnPoint.position, Quaternion.identity);
		player.GetComponent<MoveToClickPoint>().spawnPoint = spawnPoint;
        return player;
    }
}