using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.PlayerManagement
{
    /* Author: Anthony D'Alesandro
     * 
     * A singleton. Bad practice but EXTREMELY useful in scene swapping. Greatly decouples my code.
     */
    public sealed class GamePlayers
    {
        // ==== Singleton Jazz
        private static GamePlayers instance = null;
        public static GamePlayers Instance
        {
            get
            {
                if (instance == null)
                    instance = new GamePlayers();
                return instance;
            }
        }
        private GamePlayers()
        {
        }

        // ===== Variables and Methods
        #region Multiplayer
        public delegate void PlayerCallback(PlayerInput input);
        public List<PlayerInput> ConnectedPlayers { get; private set; } = new List<PlayerInput>();
        public List<InputDevice> PlayerDevices { get; private set; } = new List<InputDevice>();

        public PlayerCallback OnPlayerAdded { get; set; } = null;
        public PlayerCallback OnPlayerRemoved { get; set; } = null;

        public void AddPlayer(PlayerInput input)
        {
            //ensure no duplications
            if (!PlayerDevices.Contains(input.devices.First()))
            {
                ConnectedPlayers.Add(input);
                PlayerDevices.Add(input.devices.First());
            }
            // Handle Event
            OnPlayerAdded?.Invoke(input);
        }

        public void RemovePlayer(PlayerInput input)
        {
            int player_index = ConnectedPlayers.IndexOf(input);
            ConnectedPlayers.RemoveAt(player_index);
            PlayerDevices.RemoveAt(player_index);

            // Properly remove player
            if (input.gameObject != null)
                GameObject.Destroy(input.gameObject);

            // Handle Event
            OnPlayerRemoved.Invoke(input);
        }

        #endregion

    }
}
