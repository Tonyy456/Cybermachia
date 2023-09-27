using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Machia.UI
{
    public class LoadInMenuInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager inputManager;
        [SerializeField] private GameObject playerParent;
        [SerializeField] private List<GameObject> buttonParents;
        [SerializeField] private GameObject UIInputModulePrefab;

        /* Author: Anthony D'Alesandro
         * 
         * Initial set up of input
         */
        public void Start()
        {
            inputManager.onPlayerJoined += OnPlayerJoin;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Initial set up of input
         */
        public void OnPlayerJoin(PlayerInput action)
        {
            Debug.Log(action.gameObject.name + " joined! " + action.name);

            GameObject eventSystemObject = Instantiate(UIInputModulePrefab);
            InputSystemUIInputModule uimodule = eventSystemObject.GetComponent<InputSystemUIInputModule>();
            action.gameObject.transform.parent = this.playerParent.transform;
            eventSystemObject.transform.parent = action.gameObject.transform;
            action.uiInputModule = uimodule;
            var manager = action.gameObject.GetComponent<UIButtonTraversal>();
            if (action.playerIndex < buttonParents.Count )
            {
                manager.Initialize(action, GetButtons(buttonParents[action.playerIndex]));
            }
        }

        public List<Button> GetButtons(GameObject parent)
        {
            List<Button> buttons = new List<Button>();
            for(int i = 0; i < parent.transform.childCount; i++)
            {
                Button btn = parent.transform.GetChild(i).GetComponent<Button>();
                if (btn != null) buttons.Add(btn);
            }
            return buttons;
        }
    }
}
