using Machia.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.PlayerManagement
{
    /* Author: Anthony D'Alesandro
     * 
     * Controls button highlights and selection code for player selection menu.
     */
    public class ReadyManager : MonoBehaviour
    {
        [SerializeField] private string scene_to_connect_to = "";
        [SerializeField] private short minPlayers = 2;
        [SerializeField] private List<PlayerInput> players = new List<PlayerInput>();
        [SerializeField] private List<bool> ready_status = new List<bool>();


        /* Author: Anthony D'Alesandro
         * 
         * Subscribe to onPlayerJoin event to initialize slot to player.
         */
        private void Awake()
        {
            GamePlayers instance = GamePlayers.Instance;
            instance.OnPlayerRemoved += OnPlayerLeave;
            instance.OnPlayerAdded += OnPlayerJoin;
        }

        private void OnDestroy()
        {
            GamePlayers instance = GamePlayers.Instance;
            instance.OnPlayerRemoved -= OnPlayerLeave;
            instance.OnPlayerAdded -= OnPlayerJoin;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Lets manager know weather player index is ready;
         */
        public void SetPlayerReadyStatus(PlayerInput player, bool status)
        {
            // Set player as ready
            var player_index = players.FindIndex(x => x.playerIndex == player.playerIndex);
            ready_status[player_index] = status;

            int unready_count = ready_status.FindAll(x => x == false).Count;
            int ready_count = ready_status.Count - unready_count;

            if (unready_count == 0 && ready_count >= minPlayers)
            {
                MachiaHelper.LoadScene(scene_to_connect_to);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Controls button highlights and selection code for player selection menu.
         */
        public void OnPlayerJoin(PlayerInput input)
        {
            players.Add(input);
            ready_status.Add(false);
        }

        public void OnPlayerLeave(PlayerInput input)
        {
            var player_index = players.IndexOf(input);
            players.RemoveAt(player_index);
            ready_status.RemoveAt(player_index);
        }
    }
}
