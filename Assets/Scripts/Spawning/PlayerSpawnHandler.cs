using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;
using UnityEngine.InputSystem;

public class PlayerSpawnHandler : IPlayerConnectedHandler
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private bool onlySpawnOnce;
    public override void ConnectPlayer(PlayerInput input)
    {
        SpawnPlayer(input);
    }

    public void SpawnPlayerPt2(PlayerInput input)
    {
        int child = Random.Range(0, spawnPoints.childCount);
        Transform childTransform = spawnPoints.GetChild(child);
        input.gameObject.transform.position = childTransform.position;
        GameObject.Destroy(childTransform.gameObject);
    }

    public void SpawnPlayer(PlayerInput input)
    {
        if (onlySpawnOnce)
        {
            SpawnPlayerPt2(input);
            return;
        }
        //Find Best Match.. Best minimum distance from all targets.
        PlayerInput[] allPlayers = GameObject.FindObjectsOfType<PlayerInput>();
        Transform bestSpawn = null;
        float bestDist = 0f;
        if(allPlayers.Length == 1)
        {
            int child = Random.Range(0, spawnPoints.childCount);
            Transform childTransform = spawnPoints.GetChild(child);
            input.gameObject.transform.position = childTransform.position;
            return;
        }

        // runspeed & memory: O(numSpawns * numPlayers)
        for(int i = 0; i < spawnPoints.childCount; i++)
        {      
            Transform spawn = spawnPoints.GetChild(i);
            float distSum = 0f;
            for(int j = 0; j < allPlayers.Length; j++)
            {
                PlayerInput player = allPlayers[j];
                if (player.playerIndex == input.playerIndex) continue;
                float playerDistFromSpawn = (player.transform.position - spawn.transform.position).magnitude;
                distSum += playerDistFromSpawn;
            }
            distSum /= allPlayers.Length;
            if(bestDist < distSum)
            {
                bestDist = distSum;
                bestSpawn = spawn;
            }
        }
        input.gameObject.transform.position = bestSpawn.position;
    }



}
