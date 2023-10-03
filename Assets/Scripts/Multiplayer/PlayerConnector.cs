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
        [SerializeField] private PlayerConnectorMethodType defaultJoinMethod = PlayerConnectorMethodType.OnInput; //allow players to join using button
        [SerializeField] private PlayerInputManager unityPlayerManager;
        [SerializeField] private Transform playerParent = null;
        [SerializeField] private PlayerEvent onPlayerJoinEvent;
        [SerializeField] private PlayerEvent onPlayerQuitAction;

        public void Start()
        {
            bool setup = false;
            onPlayerQuitAction.subscription += OnPlayerLeave;
            unityPlayerManager.onPlayerJoined += OnPlayerJoin;
            unityPlayerManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

            // try to connect players if the user wants that and it worked.
            if (defaultJoinMethod == PlayerConnectorMethodType.FindDevices)
            {
                throw new NotImplementedException();
            }
            else if ( defaultJoinMethod == PlayerConnectorMethodType.UseCurrentPlayers)
            {

                GamePlayers connected = GameObject.FindObjectOfType<GamePlayers>(true);
                if (connected != null)
                {
                    setup = true;
                    var arr = connected.Devices.ToArray();
                    for(int i = 0; i < arr.Length; i++)
                    {
                        unityPlayerManager.JoinPlayer(pairWithDevice: arr[i]);
                    }
                }
            }

            // by default make it so a button allows users to join.
            if (!setup) unityPlayerManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed; 


        }

        /* Author: Anthony D'Alesandro
         * 
         * Communicates the connected player to IPlayerConnectorHandlers and IInputActors.
         */
        public void OnPlayerJoin(PlayerInput player)
        {
            GameObject go = player.gameObject;
            if (playerParent)
            {
                go.transform.parent = playerParent;
            }

            onPlayerJoinEvent?.Trigger(player);

            //Gameobject is freshly spawned so can not really subscribe to the event and get its input initialized. Best way to do this.
            foreach(var handler in go.GetComponentsInChildren<IPlayerConnectedHandler>())
            {
                handler.Initialize(player);
            }
        }

        private void OnPlayerLeave(PlayerInput input)
        {
            Debug.Log("attempt to leave player");
        }

    }
}
