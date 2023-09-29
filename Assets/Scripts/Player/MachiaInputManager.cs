using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Machia.Player;

namespace Machia.Input
{
    public class MachiaInputManager : MonoBehaviour, IInputActor
    {
        [SerializeField] private PlayerController playerController;

        //initialize player controller input
        public void Initialize(PlayerInput inputManager)
        {
            var map = inputManager.currentActionMap;
            var dash = map.FindAction("Dash");
            dash.Enable();
            var move = map.FindAction("Move");
            move.Enable();
            playerController.InitializeDash(dash);
            playerController.InitializeMove(move);
        }
    }
}
