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

        private List<PlayerInput> connectedOrder = new List<PlayerInput>();

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
            connectedOrder.Add(inputManager);
            PlayerSlotController unassigned = playerUnits.Find(x => !x.assigned);
            unassigned.AssignToPlayer(inputManager, onPlayerQuitAction, $"Player {connectedOrder.Count}");
        }

        public void RemovePlayer(PlayerInput player)
        {
            foreach (var s in playerUnits)
            {
                if (s.assigned) s.ClearData();
            }
            connectedOrder.Remove(player);
            for(int i = 0; i < connectedOrder.Count; i++)
            {
                PlayerInput toAssign = connectedOrder[i];
                playerUnits[i].AssignToPlayer(toAssign, onPlayerQuitAction, $"Player {connectedOrder.IndexOf(toAssign) + 1}");
            }
        }

    }
}
