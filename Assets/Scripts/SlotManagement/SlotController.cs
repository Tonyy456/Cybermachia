using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CyberMachia
{
    [RequireComponent(typeof(PlayerInput))]
    public class SlotController : MonoBehaviour
    {
        [SerializeField] GameObject textPrefab;
        [SerializeField] private string parentTag;
        private PlayerInput input;
        private InputAction confirm;
        private InputAction cancel;
        private InputActionMap inputMap;

        private bool ready = false;
        private ReadyManager instance;
        private TMPro.TMP_Text slot1ToChange;

        public void Start()
        {
            input = GetComponent<PlayerInput>();
            inputMap = input.currentActionMap;
            inputMap.Enable();
            confirm = inputMap.FindAction("Confirm");
            cancel = inputMap.FindAction("Cancel");
            InitializeInput();

            var parent = GameObject.FindGameObjectWithTag(parentTag);
            if (parent)
            {
                var go = GameObject.Instantiate(textPrefab);
                slot1ToChange = go.GetComponent<TMPro.TMP_Text>();
                go.transform.SetParent(parent.transform);
            }

            if(slot1ToChange)
                slot1ToChange.text = $"Player {input.playerIndex} - {input.currentControlScheme}";
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
            if (instance != null) instance.RemoveSlot(this);
            confirm.performed -= OnConfirmAction;
            cancel.performed -= OnCancelAction;
            if (slot1ToChange) Destroy(slot1ToChange.gameObject);
        }

        private void OnConfirmAction(InputAction.CallbackContext context)
        {
            //if (!ready) ReadyUp();
        }

        private void OnCancelAction(InputAction.CallbackContext context)
        {
            Destroy(this.gameObject);
            //if (ready)
            //{
            //    Unready();
            //}
            //else
            //{
            //    Destroy(this.gameObject);
            //}
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
