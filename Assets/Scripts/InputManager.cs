using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Machia.Player;


/* Author: Anthony D'Alesandro
 * 
 * Manages MachiaInputActions from Unity's new input system.
 */
namespace Machia.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager inputManager;


        /* Author: Anthony D'Alesandro
         * 
         * Initial set up of input
         */
        public void Start()
        {
            //OnPlayerJoin(inputManager.JoinPlayer(0, -1, null, InputSystem.devices[0]));
            inputManager.onPlayerJoined += OnPlayerJoin;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Initial set up of input
         */
        public void OnPlayerJoin(PlayerInput action)
        {
            Debug.Log(action.gameObject.name + " joined! " + action.name);

            var manager = action.gameObject.GetComponent<LocalPlayerInputManager>();
            if (manager == null) Debug.LogError("[ISSUE] manager is null");

            manager.AssignInput(action);
        }
    }
}
