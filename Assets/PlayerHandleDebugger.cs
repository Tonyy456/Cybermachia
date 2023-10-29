using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandleDebugger : IPlayerConnectedHandler
{
    [SerializeField] private bool disablePlayers = true;
    [SerializeField] private bool onStateEnterEnablePlayers = true;
    [SerializeField] private Tony.StateSO fightState;

    public void Start()
    {
        if (fightState != null) {
            fightState.OnEnter += OnFightState;
        }
    }

    public override void ConnectPlayer(PlayerInput input)
    {
        if(disablePlayers)
        {
            var controller = input.gameObject.GetComponent<PlayerController>();
            if(controller != null)
            {
                controller.PausePlayer();
            } else
            {
                Destroy(input.gameObject);
            }
        }
    }

    public void OnFightState()
    {
        if(onStateEnterEnablePlayers)
        {
            PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
            foreach (var player in players) player.EnablePlayer();
        }
    }
}
