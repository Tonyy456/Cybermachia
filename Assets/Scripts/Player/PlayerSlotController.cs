using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Events;
using System;

using Machia.PlayerManagement;

namespace Machia.Player
{
    /* Author: Anthony D'Alesandro
     * 
     * Input Manager for player select menu.
     */
    public class PlayerSlotController : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Selectable selected;
        [SerializeField] private TMPro.TMP_Text playerTitle;

        [Header("Settings")]
        [SerializeField] private Image imageToChange;
        [SerializeField] private Color selectedColor = Color.red;
        [SerializeField] private Color imageReadyColor;
        private Color imageDefaultColor;

        public bool assigned { get; private set; } = false;
        public bool ready { get; private set; } = false;

        private PlayerInput input;

        /* Author: Anthony D'Alesandro
         * 
         * Initializes slot in editor only. Saves work time.
         */
        public void OnValidate()
        {
            selected = this.GetComponentInChildren<Selectable>();
            if (playerTitle)
            {
                playerTitle.text = $"Press Button To Join!";
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Assigns this slot to a player. Initialize input and allows traversal through buttons.
         */
        public void AssignToPlayer(PlayerInput input, string playerName)
        {
            this.input = input;
            this.imageDefaultColor = imageToChange.color;

            if (playerTitle)  playerTitle.text = $"{playerName}";
            InitializeInput(input);
            EnableInput();
            SelectButton(selected);
            assigned = true;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Removes the player on this slot. 
         */
        public void RemovePlayer()
        {
            UnassignPlayer();
            Machia.PlayerManagement.GamePlayers.Instance.RemovePlayer(this.input);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Disables Input except cancel button and wire up cancel button
         */
        public void ToggleReady(bool status)
        {
            ready = status;

            // set color and inputs on/off
            imageToChange.color = ready ? imageReadyColor : imageDefaultColor;

            // alert manager that you are ready
            var readyManager = GameObject.FindObjectOfType<ReadyManager>(true);
            if (readyManager != null)
            {
                readyManager.SetPlayerReadyStatus(input, ready);
            }
        }

        public void UnassignPlayer()
        {
            if (playerTitle) playerTitle.text = $"Press Button To Join!";
            assigned = false;
            imageToChange.color = imageDefaultColor;
            SelectButton(null);
            InitializeInput(input, remove: true);
            this.EnableInput(false);
        }

        public void EnableInput(bool enable = true)
        {
            var map = input.currentActionMap;
            if (enable)
            {
                map.FindAction("TraverseUp").Enable();
                map.FindAction("TraverseDown").Enable();
                map.FindAction("Confirm").Enable();
                map.FindAction("Cancel").Enable();
            }
            else
            {
                map.FindAction("TraverseUp").Disable();
                map.FindAction("TraverseDown").Disable();
                map.FindAction("Confirm").Disable();
                map.FindAction("Cancel").Disable();
            }
        }

        private void InitializeInput(PlayerInput input, bool remove = false)
        {
            var map = input.currentActionMap;
            if (remove)
            {
                map.FindAction("TraverseUp").performed -= OnUp;
                map.FindAction("TraverseDown").performed -= OnDown;
                map.FindAction("Confirm").performed -= OnConfirm;
                map.FindAction("Cancel").performed -= OnCancelReady;
            } else
            {
                map.FindAction("TraverseUp").performed += OnUp;
                map.FindAction("TraverseDown").performed += OnDown;
                map.FindAction("Confirm").performed += OnConfirm;
                map.FindAction("Cancel").performed += OnCancelReady;
            }
        }

        private void OnUp(InputAction.CallbackContext e)
        {
            if (ready) return;
            var up_selectable = selected.FindSelectableOnUp();
            if (up_selectable) SelectButton(up_selectable);
        }
        private void OnDown(InputAction.CallbackContext e)
        {
            if (ready) return;
            var down_selectable = selected.FindSelectableOnDown();
            if (down_selectable) SelectButton(down_selectable);
        }

        private void OnConfirm(InputAction.CallbackContext e)
        {
            if (ready) return;
            Button btn = selected as Button;
            if (btn) btn.onClick?.Invoke();
        }

        /* Author: Anthony D'Alesandro
         * 
         * Preformed when cancel button is pressed. toggles ready off and unready's player.
         */
        public void OnCancelReady(InputAction.CallbackContext e)
        {
            if (ready)
                this.ToggleReady(false);
            else
                RemovePlayer();
        }

        private void SelectButton(Selectable button)
        {
            if (selected != null)
            {
                var text_element = selected.GetComponentInChildren<TMPro.TMP_Text>();
                if (text_element) text_element.color = Color.white;
            }
            if (button)
            {
                var text_element = button.GetComponentInChildren<TMPro.TMP_Text>();
                if (text_element) text_element.color = selectedColor;
            }
            selected = button;
        }
    }
}
