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
        [SerializeField] private int numPlayers = 0;
        [SerializeField] private List<bool> statuses = new List<bool>();

        /* Author: Anthony D'Alesandro
         * 
         * Controls button highlights and selection code for player selection menu.
         */
        void Start()
        {
            onPlayerJoin.subscription += Initialize; 
        }

        private void OnDestroy()
        {
            onPlayerJoin.subscription -= Initialize;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Lets manager know weather player index is ready;
         */
        public void SetPlayerReadyStatus(int playerIndex, bool status)
        {
            statuses[playerIndex] = status;
            var list_of_ready = statuses.FindAll(x => x == true);
            if (list_of_ready.Count > 1 && numPlayers == list_of_ready.Count)
            {
                LoadScene.LoadSceneFromName(scene_to_connect_to);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Controls button highlights and selection code for player selection menu.
         */
        public void Initialize(PlayerInput input)
        {
            numPlayers += 1;
            statuses.Add(false);
        }
    }
}
