using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Machia.UI;
using Machia.Input;
using TMPro;

public class PlayerSlotController : MonoBehaviour
{
    [SerializeField] private List<Selectable> NavButtons = new List<Selectable>();
    [SerializeField] private int playerIndex = 1;
    [SerializeField] private TMPro.TMP_Text playerTitle = null;
    [SerializeField] private Color selectedColor = Color.red;

    private UIButtonController traversal;
    public void OnValidate()
    {
        NavButtons = new List<Selectable>(this.GetComponentsInChildren<Selectable>());
        if (playerTitle)
        {
            playerTitle.text = $"Press Button To Join!";
        }
    }
    public void AssignToPlayer(PlayerInput input)
    {
        traversal = new UIButtonController(NavButtons, selectedColor, 0);
        traversal.InitializeUp(input.currentActionMap.FindAction("TraverseUp"));
        traversal.InitializeDown(input.currentActionMap.FindAction("TraverseDown"));
        traversal.InitializeConfirm(input.currentActionMap.FindAction("Confirm"));
        playerIndex = input.playerIndex;
        if (playerTitle)
        {
            playerTitle.text = $"Player {input.playerIndex + 1}";
        }
    }

    public void Clear()
    {
        if (playerTitle)
        {
            playerTitle.text = $"Unassigned";
        }
    }


}
