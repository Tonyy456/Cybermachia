using Machia.Events;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Passes data around when a player is connected. Manages already connected players.
     */
    public class PlayerConnector : MonoBehaviour
    {
        [SerializeField] private bool useAlreadyConnectedPlayers = false; //allow players to join using button
        [SerializeField] private PlayerInputManager unityPlayerManager;
        [SerializeField] private Transform playerParent = null;
        [SerializeField] private PlayerEvent onPlayerJoinEvent;

        public void Start()
        {
            if (!useAlreadyConnectedPlayers)
            {
                unityPlayerManager.onPlayerJoined += OnPlayerJoin;
            } else
            {
               // unityPlayerManager.JoinPlayer(playerIndex: -1, splitScreenIndex: -1, controlScheme: null, pairWithDevice: null);
                //handle if controllers are not connected anymore for some reason
                //handle on controller leave etc.
            }
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

    }
}
