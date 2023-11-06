using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandleDebugger : IPlayerConnectedHandler
{
    [SerializeField] private bool disablePlayers = true;
    [SerializeField] private bool onStateEnterEnablePlayers = true;
    [SerializeField] private Tony.StateSO fightState;
    [SerializeField] private List<PlayerAmmoUIController> mAmmoUIControllers = new List<PlayerAmmoUIController>();
    [SerializeField] private List<Color> playerColors = new List<Color>();

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
                var sp = mAmmoUIControllers[input.playerIndex];
                var color = playerColors[input.playerIndex];
                controller.InitializePlayerData(sp, color);
                controller.PausePlayer();
                Debug.Log("paused player");
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
