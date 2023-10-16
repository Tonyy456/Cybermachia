using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tony
{
    public delegate PlayerInput PlayerCallbackContext();
    [CreateAssetMenu(fileName = "PlayerEvent", menuName = "Event/PlayerCallback")]
    public class PlayerEventSO : ScriptableObject
    {
        public PlayerCallbackContext onPlayerEvent;
    }
}
