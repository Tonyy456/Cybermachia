using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Machia.Input
{
    public class PlayerConnector : MonoBehaviour
    {
        [SerializeField] private bool useAlreadyConnectedPlayers = false; //allow players to join using button

        [SerializeField] private PlayerInputManager unityPlayerManager;

        public void Start()
        {
            if (!useAlreadyConnectedPlayers)
            {
                unityPlayerManager.onPlayerJoined += OnPlayerJoin;
            }
        }
        public void OnPlayerJoin(PlayerInput action)
        {
            GameObject player = action.gameObject;

            // ensure prefab has input manager on it
            SubPlayerInputManager inputManager = player.GetComponent<SubPlayerInputManager>();
            if (inputManager == null)
            {
                player.AddComponent<SubPlayerInputManager>();
            }
            inputManager.ConnectInput(action);

            // give input manager to all scripts that need it.
            IInputActor[] actors = player.GetComponents<IInputActor>();
            foreach (var actor in actors)
            {
                actor.Initialize(inputManager);
            }

        }

    }
}
