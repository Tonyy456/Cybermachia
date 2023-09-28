using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Abstract Input Manager for Player.
     */
    public class SubPlayerInputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private InputActionMap inputActions;
        public void ConnectInput(PlayerInput action)
        {
            playerInput = action;
            inputActions = action.currentActionMap;
        }

        public InputAction GetAction(string name)
        {
            return inputActions.FindAction(name);
        }

        public void EnableAction(string name)
        {
            GetAction(name)?.Enable();
        }

        public void DisableAction(string name)
        {
            GetAction(name)?.Enable();
        }
    }
}
