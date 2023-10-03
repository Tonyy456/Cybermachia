using Machia.Events;
using Machia.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Controls button highlights and selection code for player selection menu.
     */
    public class ReadyManager : MonoBehaviour
    {
        [SerializeField] private string scene_to_connect_to = "";
        [SerializeField] private PlayerEvent onPlayerJoin;
        [SerializeField] private PlayerEvent onPlayerLeave;
        [SerializeField] private List<PlayerInput> player = new List<PlayerInput>();
        [SerializeField] private List<bool> ready_status = new List<bool>();


        /* Author: Anthony D'Alesandro
         * 
         * Controls button highlights and selection code for player selection menu.
         */
        void Start()
        {
            onPlayerLeave.subscription += OnPlayerLeave;
            onPlayerJoin.subscription += OnPlayerJoin; 
        }

        private void OnDestroy()
        {
            onPlayerLeave.subscription -= OnPlayerLeave;
            onPlayerJoin.subscription -= OnPlayerJoin;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Lets manager know weather player index is ready;
         */
        public void SetPlayerReadyStatus(int playerIndex, bool status)
        {
            var player_index = player.FindIndex(x => x.playerIndex == playerIndex);
            ready_status[player_index] = status;
            int unready_count = ready_status.FindAll(x => x == false).Count;

            if (unready_count == 0)
            {
                LoadScene.LoadSceneFromName(scene_to_connect_to);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Controls button highlights and selection code for player selection menu.
         */
        public void OnPlayerJoin(PlayerInput input)
        {
            player.Add(input);
            ready_status.Add(false);
        }

        public void OnPlayerLeave(PlayerInput input)
        {
            var player_index = player.IndexOf(input);
            player.RemoveAt(player_index);
            ready_status.RemoveAt(player_index);
        }
    }
}
