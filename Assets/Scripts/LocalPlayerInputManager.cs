using System.Collections;
using System.Collections.Generic;
using Machia.Input;
using Machia.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerInputManager : MonoBehaviour
{
    private PlayerInput input;
    private InputAction moveAction;
    private InputAction dashAction;


    /* Author: Anthony D'Alesandro
     * 
     * Initial set up of input
     */
    public void Awake() { }

    /* Author: Anthony D'Alesandro
     * 
     * Initialize input, must be called somewhere else.
     */
    public void AssignInput(PlayerInput inp)
    {
        Debug.Log("Initializing local input. Finding InputActions.");
        if (inp == null) Debug.LogError("[ISSUE] null PlayerInput action");

        input = inp;
        var map = input.currentActionMap;
        moveAction = map.FindAction("Move");
        dashAction = map.FindAction("Dash");

        if (moveAction == null) Debug.LogError("[ISSUE] null move action");
        if (dashAction == null) Debug.LogError("[ISSUE] null dash action");

        var controller = this.GetComponent<PlayerController>();
        if (controller == null) Debug.LogError("[ISSUE] controller is null");
        controller.InitializeInput(this);
    }

    /* Author: Anthony D'Alesandro
     * 
     * Allow toggling section of Input Actions
     */
    public void EnableMinigameInput() => input?.currentActionMap.Enable();
    public void DisableMinigameInput() => input?.currentActionMap.Disable();


    /* Author: Anthony D'Alesandro
     * 
     * Allow enabling and disabling of inputs
     */
    public void EnableMove() => moveAction?.Enable();
    public void DisableMove() => moveAction?.Disable();

    public void EnableDash() => dashAction?.Enable();
    public void DisableDash() => dashAction?.Disable();


    /* Author: Anthony D'Alesandro
     * 
     * Get input actions from MachiaInput system.
     */
    public InputAction MoveAction { get => moveAction; }
    public InputAction DashAction { get => dashAction; }
    public Vector2 MoveDir { get => MoveAction.ReadValue<Vector2>(); }
}
