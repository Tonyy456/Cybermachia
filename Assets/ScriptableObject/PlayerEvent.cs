using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Events
{
    /* Author: Anthony D'Alesandro
     * 
     * Delegate used in PlayerEvent that gives the player input to other scripts.
     */
    public delegate void PlayerCallback(PlayerInput input);

    /* Author: Anthony D'Alesandro
     * 
     * Passes data around when a player is connected. Manages already connected players.
     */
    [CreateAssetMenu(fileName = "PlayerEvent", menuName = "Player Event")]
    public class PlayerEvent : ScriptableObject
    {
        public PlayerCallback subscription { get; set; }
        public void Trigger(PlayerInput input)
        {
            subscription?.Invoke(input);
        }
    }
}
