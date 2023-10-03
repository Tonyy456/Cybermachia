using Machia.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Potential place to store connected players and move them between scenes.
     */
    public class GamePlayers : MonoBehaviour
    {
        [SerializeField] private PlayerEvent onPlayerJoin;
        public List<InputDevice> Devices { get; private set; } = new List<InputDevice>();

        public void Awake()
        {
            if (onPlayerJoin != null) onPlayerJoin.subscription += playerJoined;
            DontDestroyOnLoad(this);
        }

        private void playerJoined(PlayerInput input)
        {
            InputDevice connected_to = input.devices.First();
            if (connected_to != null)
            {
                Devices.Add(connected_to);
            }
        }
    }
}
