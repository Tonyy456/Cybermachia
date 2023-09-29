using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Handles a controller joining in the character select menu.
     */
    public class MenuPlayerConnectorHandler : MonoBehaviour, IPlayerConnectorHandler
    {
        [SerializeField] private List<PlayerSlotController> playerUnits;

        /* Author: Anthony D'Alesandro
         * 
         * Called when OnPlayerJoin is triggered from PlayerInputManager. Referenced in PlayerConnector
         */
        public void Initialize(PlayerInput inputManager)
        {
            Debug.Log("New player: " + inputManager.playerIndex);
            playerUnits[inputManager.playerIndex].AssignToPlayer(inputManager);
        }
    }
}
