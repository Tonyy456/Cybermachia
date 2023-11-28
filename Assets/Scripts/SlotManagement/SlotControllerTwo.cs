using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CyberMachia
{
    [RequireComponent(typeof(PlayerInput))]
    public class SlotControllerTwo : MonoBehaviour
    {
        private PlayerInput input;
        private InputAction confirm;
        private InputAction cancel;
        private InputActionMap inputMap;

        private bool ready = false;
        private ReadyManager instance;

        public void Start()
        {
            input = GetComponent<PlayerInput>();
            inputMap = input.currentActionMap;
            inputMap.Enable();
            confirm = inputMap.FindAction("Confirm");
            cancel = inputMap.FindAction("Cancel");
            InitializeInput();
        }

        private void InitializeInput()
        {
            SetActionStatus(confirm);
            SetActionStatus(cancel);
            confirm.performed += this.OnConfirmAction;
            cancel.performed += this.OnCancelAction;
        }
        private void OnDestroy()
        {
            confirm.performed -= OnConfirmAction;
            cancel.performed -= OnCancelAction;
        }

        private void OnConfirmAction(InputAction.CallbackContext context)
        {
            if (!ready) ReadyUp();
        }

        private void OnCancelAction(InputAction.CallbackContext context)
        {
            if (ready)
            {
                Unready();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void ReadyUp()
        {
            ready = true;
        }

        public void Unready()
        {
            ready = false;

        }

        private void SetActionStatus(InputAction action, bool status = true)
        {
            if (status)
            {
                action.Enable();
            }
            else
            {
                action.Disable();
            }
        }
    }
}
