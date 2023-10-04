using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Machia.Player;

namespace Machia.PlayerManagement
{
    /* Author: Anthony D'Alesandro
     * 
     * Handles a controller joining in the character select menu.
     */
    public class MenuPlayerConnectorHandler : MonoBehaviour
    {
        [SerializeField] private List<PlayerSlotController> playerUnits;

        /* Author: Anthony D'Alesandro
         * 
         * Subscribe to onPlayerJoin event to initialize slot to player.
         */
        private void Awake()
        {
            GamePlayers instance = GamePlayers.Instance;
            instance.OnPlayerRemoved += ShiftPlayersDown;
            instance.OnPlayerAdded += AddPlayerToSlot;
        }

        private void OnDestroy()
        {
            GamePlayers instance = GamePlayers.Instance;
            instance.OnPlayerRemoved -= ShiftPlayersDown;
            instance.OnPlayerAdded -= AddPlayerToSlot;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Called when OnPlayerJoin is triggered from PlayerInputManager. Referenced in PlayerConnector
         */
        public void AddPlayerToSlot(PlayerInput inputManager)
        {
            GamePlayers instance = GamePlayers.Instance;
            PlayerSlotController unassigned = playerUnits.Find(x => !x.assigned);
            unassigned.AssignToPlayer(inputManager, $"Player {instance.ConnectedPlayers.Count}");
        }

        public void ShiftPlayersDown(PlayerInput player)
        {
            foreach (var s in playerUnits)
            {
                if (s.assigned) s.UnassignPlayer();
            }
            GamePlayers instance = GamePlayers.Instance;
            for (int i = 0; i < instance.ConnectedPlayers.Count; i++)
            {
                PlayerInput toAssign = instance.ConnectedPlayers[i];
                playerUnits[i].AssignToPlayer(toAssign, $"Player {i + 1}");
            }
        }

    }
}
