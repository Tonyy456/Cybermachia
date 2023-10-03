using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Controls button highlights and selection code for player selection menu.
     */
    public class UIButtonController
    {
        private List<Selectable> buttons;
        private Selectable current = null;
        private Color selectedColor;
        public UIButtonController(List<Selectable> controlButtons, Color selectedColor, int startButton = 0)
        {
            buttons = controlButtons;
            this.selectedColor = selectedColor;
            current = controlButtons[startButton];
            SelectButton(current);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Connects OnUp function to the InputAction.
         */
        public void InitializeUp(InputAction action, bool remove = false)
        {
            if (!remove)
            {
                action.Enable();
                action.performed += OnUp;
            } else
            {
                action.Disable();
                action.performed -= OnUp;
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Connects OnDown function to InputAction.
         */
        public void InitializeDown(InputAction action, bool remove = false)
        {
            if (!remove)
            {
                action.Enable();
                action.performed += OnDown;
            }
            else
            {
                action.Disable();
                action.performed -= OnDown;
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Connects OnConfirm function to InputAction.
         */
        public void InitializeConfirm(InputAction action, bool remove = false)
        {
            if (!remove)
            {
                action.Enable();
                action.performed += OnConfirm;
            }
            else
            {
                action.Disable();
                action.performed -= OnConfirm;
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handles "Confirm" action for player when button is selected.
         */
        public void OnConfirm(InputAction.CallbackContext e)
        {
            Button btn = current as Button;
            if (btn)
            {
                btn.onClick?.Invoke();
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handles "Up" action for player when player tries moving around the UI elements.
         */
        public void OnUp(InputAction.CallbackContext e)
        {
            var up_selectable = current.FindSelectableOnUp();
            if (up_selectable)
            {
                SelectButton(up_selectable);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handles "Down" action for player when player tries moving around the UI elements.
         */
        public void OnDown(InputAction.CallbackContext e)
        {
            var down_selectable = current.FindSelectableOnDown();
            if (down_selectable)
            {
                SelectButton(down_selectable);
            }
        }

        public void Clear()
        {
            foreach(var button in buttons)
            {
                var item = button.GetComponentInChildren<TMPro.TMP_Text>();
                if (item) item.color = Color.white;
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handles "Select" action for player when they press the corresponding button.
         */
        private void SelectButton(Selectable button)
        {
            var item = current.GetComponentInChildren<TMPro.TMP_Text>();
            if (item) item.color = Color.white;

            current = button;
            var item2 = button.GetComponentInChildren<TMPro.TMP_Text>();
            if (item2) item2.color = selectedColor;
        }
    }
}
