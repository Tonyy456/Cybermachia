using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class TIL_MovementController : MonoBehaviour, PlayerInputScript
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashLength = 1f;
    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private Animator animator;

    public bool Enabled { get; set; } = false;
    public UnityEvent OnDashStart { get; set; }
    public UnityEvent OnDashFinish { get; set; }

    private PlayerInput input;
    private InputAction move;
    private InputAction dash;
    private bool inDash = false;
    private Vector2 targetDashDirection = Vector2.zero;
    private float lastDash;

    public void Start()
    {
        input = this.GetComponent<PlayerInput>();
        InputActionMap map = input.currentActionMap;
        InitializeMove(map.FindAction("Move"));
        InitializeDash(map.FindAction("ActionTwo"));
    }

    public void EnableInput()
    {
        Enabled = true;
        if (move == null || dash == null) return;
        move.Enable();
        dash.Enable();
    }

    public void DisableInput()
    {
        Enabled = false;
        if (move == null || dash == null) return;
        move.Disable();
        dash.Disable();
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        if (move == null || inDash) return;
        Vector2 dp = move.ReadValue<Vector2>();
        Vector3 updateP = new Vector3(dp.x, dp.y, 0);
        SetMovementAnimation(dp);
        this.transform.position += (updateP * Time.deltaTime * speed);
    }

    private void SetMovementAnimation(Vector2 movementDir)
    {
        if (movementDir.x > 0.1)
        {
            animator.Play("WalkRight");
        }
        else if (movementDir.x < -0.1)
        {
            animator.Play("WalkLeft");
        }
        else
        {
            if (movementDir.y > 0.1)
            {
                animator.Play("WalkUp");
            }
            else if (movementDir.y < -0.1)
            {
                animator.Play("WalkDown");
            }
            else
            {
                animator.Play("Idle");
            }
        }
    }

    private IEnumerator DashRoutine()
    {
        float current = Time.time;
        while ((Time.time - current) < dashLength)
        {
            Vector2 dp = targetDashDirection;
            Vector3 updateP = new Vector3(dp.x, dp.y, 0);
            this.transform.position += (updateP * Time.deltaTime * dashSpeed);
            yield return new WaitForEndOfFrame();
        }
        //Clean up
        inDash = false;
        OnDashFinish?.Invoke();
        yield return null;
    }

    private void DashHandler(InputAction.CallbackContext callback)
    {
        if ((Time.time - lastDash) < dashDelay) return;
        lastDash = Time.time;
        targetDashDirection = move.ReadValue<Vector2>();

        if (targetDashDirection.magnitude > 0.05f)
        {
            inDash = true;
            OnDashStart?.Invoke();
            StartCoroutine(DashRoutine());
        }
    }

    private void InitializeDash(InputAction action)
    {
        dash = action;
        dash.performed += DashHandler;
        if (Enabled) dash.Enable();
        else dash.Disable();
    }

    private void InitializeMove(InputAction action)
    {
        move = action;
        if (Enabled) move.Enable();
        else move.Disable();
    }
}
