using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;
using Machia.Events;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Input Manager for player select menu.
     */
    public class PlayerSlotController : MonoBehaviour
    {
        [SerializeField] private List<Selectable> NavButtons = new List<Selectable>();
        [SerializeField] private int playerIndex = 1;
        [SerializeField] private TMPro.TMP_Text playerTitle = null;
        [SerializeField] private Color selectedColor = Color.red;
        [SerializeField] private Color readyColor;

        public bool assigned { get; private set; } = false;

        private UIButtonController traversal;
        private PlayerInput input;
        private PlayerEvent onPlayerQuit;
        private bool ready = false;
        private Color defaultReadyColor;

        /* Author: Anthony D'Alesandro
         * 
         * Initializes slot in editor only. Saves work time.
         */
        public void OnValidate()
        {
            NavButtons = new List<Selectable>(this.GetComponentsInChildren<Selectable>());
            if (playerTitle)
            {
                playerTitle.text = $"Press Button To Join!";
            }
            onPlayerQuit?.Trigger(this.input);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Assigns this slot to a player. Initialize input and allows traversal through buttons.
         */
        public void AssignToPlayer(PlayerInput input, PlayerEvent onPlayerQuit, string playerName)
        {
            this.input = input;
            this.onPlayerQuit = onPlayerQuit;
            traversal = new UIButtonController(NavButtons, selectedColor, 0);
            traversal.InitializeUp(input.currentActionMap.FindAction("TraverseUp"));
            traversal.InitializeDown(input.currentActionMap.FindAction("TraverseDown"));
            traversal.InitializeConfirm(input.currentActionMap.FindAction("Confirm"));
            this.defaultReadyColor = this.GetComponent<Image>().color;
            playerIndex = input.playerIndex;
            if (playerTitle)
            {
                playerTitle.text = $"{playerName}";
            }
            InputAction cancel = input.currentActionMap.FindAction("Cancel");
            cancel.performed += OnCancelReady;
            cancel.Enable();
            assigned = true;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Removes the player on this slot. 
         */
        public void PlayerDisconnect()
        {
            ClearData();
            onPlayerQuit?.Trigger(this.input);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Disables Input except cancel button and wire up cancel button
         */
        public void ToggleReady(bool status)
        {
            ready = status;

            // set color and inputs on/off
            this.gameObject.GetComponent<Image>().color = ready ? readyColor : defaultReadyColor;
            SetInputAction(input.currentActionMap.FindAction("TraverseUp"), !ready);
            SetInputAction(input.currentActionMap.FindAction("TraverseDown"), !ready);
            SetInputAction(input.currentActionMap.FindAction("Confirm"), !ready);

            // alert manager that you are ready
            var readyManager = GameObject.FindObjectOfType<ReadyManager>(true);
            if (readyManager != null)
            {
                readyManager.SetPlayerReadyStatus(input.playerIndex, ready);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Preformed when cancel button is pressed. toggles ready off and unready's player.
         */
        public void OnCancelReady(InputAction.CallbackContext e)
        {
            if(ready)
                this.ToggleReady(false);
            else
            {
                PlayerDisconnect();
            }
                
        }

        /* Author: Anthony D'Alesandro
         * 
         * Sets input action to a status of @code{on}
         */
        private void SetInputAction(InputAction action, bool on)
        {
            if (on) action.Enable();
            else action.Disable();
        }

        public void ClearData()
        {
            if (playerTitle)
            {
                playerTitle.text = $"Press Button To Join!";
            }
            playerIndex = 0;
            assigned = false;
            this.gameObject.GetComponent<Image>().color = defaultReadyColor;
            traversal.InitializeUp(input.currentActionMap.FindAction("TraverseUp"), true);
            traversal.InitializeDown(input.currentActionMap.FindAction("TraverseDown"), true);
            traversal.InitializeConfirm(input.currentActionMap.FindAction("Confirm"), true);
            traversal.Clear();
            InputAction cancel = input.currentActionMap.FindAction("Cancel");
            cancel.performed -= OnCancelReady;
            cancel.Disable();
        }
    }
}
