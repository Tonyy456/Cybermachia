using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace Machia.UI
{
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
        }

        public void InitializeUp(InputAction action)
        {
            action.Enable();
            action.performed += OnUp;
        }
        public void InitializeDown(InputAction action)
        {
            action.Enable();
            action.performed += OnDown;
        }

        public void InitializeConfirm(InputAction action)
        {
            action.Enable();
            action.performed += OnConfirm;
        }

        public void OnConfirm(InputAction.CallbackContext e)
        {
            Button btn = current as Button;
            Debug.Log(btn);
            if (btn)
            {
                btn.onClick?.Invoke();
            }
        }

        public void OnUp(InputAction.CallbackContext e)
        {
            var up_selectable = current.FindSelectableOnUp();
            if (up_selectable)
            {
                SelectButton(up_selectable);
            }
        }

        public void OnDown(InputAction.CallbackContext e)
        {
            var down_selectable = current.FindSelectableOnDown();
            if (down_selectable)
            {
                SelectButton(down_selectable);
            }
        }

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
