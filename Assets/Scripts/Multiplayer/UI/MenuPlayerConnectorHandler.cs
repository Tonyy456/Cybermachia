using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Machia.Input;
using UnityEngine.EventSystems;
using System;

namespace Machia.UI
{
    public class MenuPlayerConnectorHandler : MonoBehaviour, IPlayerConnectorHandler
    {
        [SerializeField] private List<PlayerSlotController> playerUnits;
        public void Initialize(PlayerInput inputManager)
        {
            Debug.Log(inputManager.playerIndex);
            playerUnits[inputManager.playerIndex].AssignToPlayer(inputManager);
        }
    }
}
