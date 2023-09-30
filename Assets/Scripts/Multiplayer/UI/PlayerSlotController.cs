using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

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

        private UIButtonController traversal;
        private PlayerInput input;
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
        }

        /* Author: Anthony D'Alesandro
         * 
         * Assigns this slot to a player. Initialize input and allows traversal through buttons.
         */
        public void AssignToPlayer(PlayerInput input)
        {
            this.input = input;
            traversal = new UIButtonController(NavButtons, selectedColor, 0);
            traversal.InitializeUp(input.currentActionMap.FindAction("TraverseUp"));
            traversal.InitializeDown(input.currentActionMap.FindAction("TraverseDown"));
            traversal.InitializeConfirm(input.currentActionMap.FindAction("Confirm"));
            this.defaultReadyColor = this.GetComponent<Image>().color;
            playerIndex = input.playerIndex;
            if (playerTitle)
            {
                playerTitle.text = $"Player {input.playerIndex + 1}";
            }
            input.currentActionMap.FindAction("CancelReady").performed += OnCancelReady;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Removes the player on this slot. 
         */
        public void Clear()
        {
            if (playerTitle)
            {
                playerTitle.text = $"Unassigned";
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Disables Input except cancel button and wire up cancel button
         */
        public void ToggleReady()
        {
            ready = !ready;
            if (input == null) return;
            if (ready) {
                this.gameObject.GetComponent<Image>().color = readyColor;
                input.currentActionMap.FindAction("TraverseUp").Disable();
                input.currentActionMap.FindAction("TraverseDown").Disable();
                input.currentActionMap.FindAction("Confirm").Disable();
                input.currentActionMap.FindAction("CancelReady").Enable(); 
            }
            else {
                this.gameObject.GetComponent<Image>().color = defaultReadyColor;
                input.currentActionMap.FindAction("TraverseUp").Enable();
                input.currentActionMap.FindAction("TraverseDown").Enable();
                input.currentActionMap.FindAction("Confirm").Enable();
                input.currentActionMap.FindAction("CancelReady").Disable();
            }

            var readyManager = GameObject.FindObjectOfType<ReadyManager>(true);
            if (readyManager != null)
            {
                readyManager.SetPlayerReadyStatus(playerIndex, ready);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Preformed when cancel button is pressed. toggles ready off and unready's player.
         */
        public void OnCancelReady(InputAction.CallbackContext e)
        {
            this.ToggleReady();
        }
    }
}
