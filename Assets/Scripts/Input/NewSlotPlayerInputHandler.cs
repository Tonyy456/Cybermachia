using UnityEngine;
using System.Collections;
using Machia.Player;
using UnityEngine.InputSystem;

public class NewSlotPlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerSlotController slotController;

    /* Author: Anthony D'Alesandro
     * 
     * Initializes InputAction to controllers.
     */
    public void Start()
    {
        PlayerInput input = this.gameObject.GetComponent<PlayerInput>();
        if (input)
        {
            slotController.AssignToPlayer(input, $"Player {input.playerIndex + 1}");
        }
    }
}

