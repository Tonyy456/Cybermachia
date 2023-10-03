using Machia.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Handles a controller joining in the character select menu.
     */
    public class MenuPlayerConnectorHandler : MonoBehaviour
    {
        [SerializeField] private List<PlayerSlotController> playerUnits;
        [SerializeField] private PlayerEvent onPlayerJoin;
        [SerializeField] private PlayerEvent onPlayerQuitAction;

        /* Author: Anthony D'Alesandro
         * 
         * Subscribe to onPlayerJoin event to initialize slot to player.
         */
        private void Awake()
        {
            onPlayerQuitAction.subscription += RemovePlayer;
            onPlayerJoin.subscription += Initialize;
        }

        private void OnDestroy()
        {
            onPlayerQuitAction.subscription -= RemovePlayer;
            onPlayerJoin.subscription -= Initialize;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Called when OnPlayerJoin is triggered from PlayerInputManager. Referenced in PlayerConnector
         */
        public void Initialize(PlayerInput inputManager)
        {
            Debug.Log("New player: " + inputManager.playerIndex);
            playerUnits[inputManager.playerIndex].AssignToPlayer(inputManager, onPlayerQuitAction);
        }

        public void RemovePlayer(PlayerInput player)
        {

        }

    }
}
