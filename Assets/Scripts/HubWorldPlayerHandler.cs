using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;
using UnityEngine.InputSystem;

public class HubWorldPlayerHandler : IPlayerConnectedHandler
{
    [SerializeField] private List<Vector2> spawnLocations;
    public override void ConnectPlayer(PlayerInput input)
    {
        Transform player = input.transform;
        Vector2 pos = spawnLocations.Count > input.playerIndex ?
            spawnLocations[input.playerIndex] : Vector2.zero;
        float oldZ = player.transform.position.z;
        player.position = new Vector3(pos.x, pos.y, oldZ);
    }
}
