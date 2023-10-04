using Machia.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Machia.Helper;
using System;

namespace Machia.Input
{
    public enum PlayerConnectorMethodType
    {
        OnInput,
        UseCurrentPlayers,
        FindDevices
    }

    /* Author: Anthony D'Alesandro
     * 
     * Passes data around when a player is connected. Manages already connected players.
     */
    public class PlayerConnector : MonoBehaviour
    {
        [Header("SCRIPTS")]
        [SerializeField] private PlayerInputManager inputManager;

        [Header("EVENTS")]
        [SerializeField] private PlayerEvent playerJoinEvent;
        [SerializeField] private PlayerEvent playerTriggeredLeave;

        [Header("SETTINGS")]
        [SerializeField] private PlayerConnectorMethodType defaultMethod = PlayerConnectorMethodType.UseCurrentPlayers;
        [SerializeField] private bool useInputIfMethodFails = true;

        private Transform parent;

        public int NumPlayersConnected { get; private set; } = 0;

        private void OnValidate()
        {
            if (inputManager == null) inputManager = this.GetComponent<PlayerInputManager>();
        }

        private void Start()
        {
            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            inputManager.onPlayerJoined += OnPlayerJoin;
            playerTriggeredLeave.subscription += OnPlayerLeave;

            bool methodFailed = true;
            if (defaultMethod == PlayerConnectorMethodType.FindDevices)
            {
                throw new NotImplementedException();
            }
            else if (defaultMethod == PlayerConnectorMethodType.UseCurrentPlayers)
            {
                // sees if players were connected in previous scene and connects them.
                GamePlayers connected = GameObject.FindObjectOfType<GamePlayers>(true);
                if (connected != null)
                {
                    var arr = connected.Devices.ToArray();
                    if (arr.Length > 0) methodFailed = false;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        inputManager.JoinPlayer(pairWithDevice: arr[i]);
                    }
                }
            }

            if (methodFailed && useInputIfMethodFails)
            {
                inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Communicates the connected player to IPlayerConnectorHandlers and IInputActors.
         */
        public void OnPlayerJoin(PlayerInput player)
        {
            GameObject go = player.gameObject;
            if (parent)
            {
                go.transform.parent = parent;
            }

            playerJoinEvent?.Trigger(player);

            //Gameobject is freshly spawned so can not really subscribe to the event and get its input initialized. Best way to do this.
            foreach (var handler in go.GetComponentsInChildren<IPlayerConnectedHandler>())
            {
                handler.Initialize(player);
            }
        }

        private void OnPlayerLeave(PlayerInput input)
        {
            GameObject.Destroy(input.gameObject);
        }
    }
}
