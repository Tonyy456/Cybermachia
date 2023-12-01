using System.Collections;
using System.Collections.Generic;
using Tony;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CyberMachia
{
    [RequireComponent(typeof(PlayerInput))]
    public class SlotController : MonoBehaviour
    {
        [SerializeField] MaterialHolder playerColorMaterial;
        [SerializeField] private string parentTag;
        [Tooltip("{0} for player index and {1} for control scheme name")]
        [SerializeField] private string formatString;
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
                var go = parent.transform.GetChild(input.playerIndex);
                slot1ToChange = go.GetComponent<TMPro.TMP_Text>();
            }

            if (slot1ToChange)
            {
                slot1ToChange.gameObject.SetActive(true);
                slot1ToChange.text = string.Format(formatString, input.playerIndex + 1, input.currentControlScheme);
                Color toUse = playerColorMaterial.playerMaterials[input.playerIndex].GetColor("_OutlineColor");
                toUse.a = 1;
                slot1ToChange.color = toUse;
            }
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
            if (slot1ToChange) slot1ToChange.gameObject.SetActive(false);
        }

        private void OnConfirmAction(InputAction.CallbackContext context)
        {
            //if (!ready) ReadyUp();
        }

        private void OnCancelAction(InputAction.CallbackContext context)
        {
            var item = GameObject.FindObjectOfType<PlayerConnector>();
            if (item) item.RemovePlayer(input);
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
