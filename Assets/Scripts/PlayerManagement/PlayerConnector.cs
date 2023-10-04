using UnityEngine;
using System.Collections;
using Machia.Input;
using System;
using UnityEngine.InputSystem;
using Machia;

namespace Machia.PlayerManagement
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerConnector : MonoBehaviour
    {
        [Header("OPTIONS")]
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private Transform Parent;

        private void OnValidate()
        {
            if (inputManager == null) inputManager = this.GetComponent<PlayerInputManager>();
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handles joining players if possible, 
         * otherwise it sets up future player's joining abilities.
         */
        private void Start()
        {
            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            inputManager.onPlayerJoined += OnPlayerJoin;

            // sees if players were connected in previous scene and connects them.
            GamePlayers instance = GamePlayers.Instance;

            var devices = instance.PlayerDevices;
            for (int i = 0; i < devices.Count; i++)
            {
                inputManager.JoinPlayer(pairWithDevice: devices[i]);
            }

            // Allow new players if not enough were connected for proper play.
            if (devices.Count < 2)
                inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Completely handles all player connection to the code base.
         * Communicates the connected player to IPlayerConnectorHandlers and IInputActors.
         */
        public void OnPlayerJoin(PlayerInput player)
        {
            GameObject go = player.gameObject;
            if (Parent)
            {
                go.transform.parent = Parent;
            }

            GamePlayers instance = GamePlayers.Instance;
            instance.AddPlayer(player);
        }
    }
}

