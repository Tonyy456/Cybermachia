using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Machia.Player;
using Machia.Events;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Handles the core player movement. Manages the Dash and Move command.
     * TODO: Rename when move action maps get passed around for different minigames?
     */
    public class MachiaInputManager : MonoBehaviour, IPlayerConnectedHandler
    {
        [SerializeField] private PlayerController playerController;

        /* Author: Anthony D'Alesandro
         * 
         * Initializes InputAction to controllers.
         */
        public void Initialize(PlayerInput player)
        {
            var map = player.currentActionMap;
            var dash = map.FindAction("Dash");
            dash.Enable();
            var move = map.FindAction("Move");
            move.Enable();
            playerController.InitializeDash(dash);
            playerController.InitializeMove(move);
        }
    }
}
