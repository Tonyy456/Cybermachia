using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Machia.Input;
using UnityEngine.EventSystems;

namespace Machia.UI
{
    public class LoadInMenuInputManager : MonoBehaviour, IPlayerConnectorHandler
    {
        [SerializeField] private List<Selectable> PlayerDefaultSelected;
        public void Initialize(PlayerInput inputManager)
        {
            var item = inputManager.gameObject.transform.GetChild(0).GetComponent<EventSystem>();
            var item2 = inputManager.gameObject.transform.GetChild(0).GetComponent<InputSystemUIInputModule>();
            item.SetSelectedGameObject(PlayerDefaultSelected[inputManager.playerIndex].gameObject);
            //throw new System.NotImplementedException();
        }
    }
}
