using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetPlayerColor : IPlayerConnectedHandler
{
    [Header("WARNING! Need one for each player or game sad :(")]
    [SerializeField] private List<Material> playerMaterials;

    public override void ConnectPlayer(PlayerInput input)
    {
        SpriteRenderer renderer = input.GetComponentInChildren<SpriteRenderer>();
        renderer.material = playerMaterials[input.playerIndex];
    }
}
