using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace Machia.UI
{
    public class UIButtonTraversal : MonoBehaviour
    {
        private PlayerInput input;
        private InputActionMap map;

        List<Button> buttons;
        private int currentSelected;
        private EventSystem eventSystem;

        private InputAction up;
        private InputAction down;
        public void Initialize(PlayerInput input, List<Button> buttons)
        {
            this.buttons = buttons;
            this.input = input;
            map = this.input.currentActionMap;

            map.Enable();
            up = map.FindAction("TraverseUp");
            down = map.FindAction("TraverseDown");

            eventSystem = input.uiInputModule.GetComponent<EventSystem>();
            SelectButton(buttons[0]);

            up.Enable();
            up.performed += OnUp;
            down.Enable();
            down.performed += OnDown;
        }

        public void OnUp(InputAction.CallbackContext e) => MoveSelected(-1);

        public void OnDown(InputAction.CallbackContext e) => MoveSelected(1);

        public void MoveSelected(int d)
        {
            currentSelected = (currentSelected + d + buttons.Count) % buttons.Count;
            Debug.Log(currentSelected);
            SelectButton(buttons[currentSelected]);
        }

        private void SelectButton(Selectable select)
        {
            if (select == null) return;
            eventSystem.SetSelectedGameObject(select.gameObject);
        }
    }
}
