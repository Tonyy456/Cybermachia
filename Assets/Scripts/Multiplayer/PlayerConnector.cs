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
            } else
            {
                //donotdestroyonload script GamePlayers will probably be in scene. check if it is and load players this way.
                //handle if controllers are not connected anymore for some reason
                //handle on controller leave etc.
            }
        }
        public void OnPlayerJoin(PlayerInput action)
        {
            GameObject player = action.gameObject;

            //// ensure prefab has input manager on it
            //SubPlayerInputManager inputManager = player.GetComponent<SubPlayerInputManager>();
            //if (inputManager == null)
            //{
            //    inputManager = player.AddComponent<SubPlayerInputManager>();
            //}
            ////input manager gets player input module
            //inputManager.ConnectInput(action);

            // give input manager to all scripts that need it.
            IInputActor[] actors = player.GetComponents<IInputActor>();
            foreach (var actor in actors)
            {
                actor.Initialize(action);
            }

        }

    }
}
