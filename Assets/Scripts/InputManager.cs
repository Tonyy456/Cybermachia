using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/* Author: Anthony D'Alesandro
 * 
 * Manages MachiaInputActions from Unity's new input system.
 */
namespace Machia.Input
{
    public class InputManager : MonoBehaviour
    {
        private MachiaInputActions input;


        /* Author: Anthony D'Alesandro
         * 
         * Initial set up of input
         */
        public void Awake()
        {
            input = new MachiaInputActions();
            EnableMinigameInput();
        }

        /* Author: Anthony D'Alesandro
         * 
         * Allow toggling section of Input Actions
         */
        public void EnableMinigameInput() => input.MinigameInput.Enable();
        public void DisableMinigameInput() => input.MinigameInput.Disable();


        /* Author: Anthony D'Alesandro
         * 
         * Allow enabling and disabling of inputs
         */
        public void EnableMove() => input.MinigameInput.Move.Enable();
        public void DisableMove() => input.MinigameInput.Move.Disable();

        public void EnableDash() => input.MinigameInput.Dash.Enable();
        public void DisableDash() => input.MinigameInput.Dash.Disable();


        /* Author: Anthony D'Alesandro
         * 
         * Get input actions from MachiaInput system.
         */
        public InputAction MoveAction { get => input.MinigameInput.Move; }
        public InputAction DashAction { get => input.MinigameInput.Dash; }
        public Vector2 MoveDir { get => MoveAction.ReadValue<Vector2>(); }


    }
}
