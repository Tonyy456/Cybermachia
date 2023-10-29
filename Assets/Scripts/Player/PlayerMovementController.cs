using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput), typeof(PlayerState))]
public class PlayerMovementController : MonoBehaviour
{
    //Movement Handling
    [Header("MOVEMENT")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashLength = 1f;
    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private bool canMoveInDash = false;

    private PlayerInput input;
    private InputAction move;
    private InputAction dash;

    private PlayerState state;

    private bool inDash = false;
    private Vector2 lastMoveDir = Vector2.zero;
    private float lastDash;

    public void Awake()
    {
        lastDash = Time.time - 1f;
    }

    public void Start()
    {
        state = this.GetComponent<PlayerState>();
        input = this.GetComponent<PlayerInput>();
        InitializeMove(input.currentActionMap.FindAction("Move"));
        InitializeDash(input.currentActionMap.FindAction("ActionTwo"));
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        if (inDash && !canMoveInDash) return;
        Vector2 dp = move.ReadValue<Vector2>();
        Vector3 updateP = new Vector3(dp.x, dp.y, 0);
        this.transform.position += (updateP * Time.deltaTime * speed);
    }

    private IEnumerator DashRoutine()
    {
        if (lastMoveDir.magnitude < 0.05f) yield return null;
        inDash = true;
        state.CanShoot = false;
        state.Invulnerable = true;
        float current = Time.time;
        while ((Time.time - current) < dashLength)
        {
            Vector2 dp = lastMoveDir;
            Vector3 updateP = new Vector3(dp.x, dp.y, 0);
            this.transform.position += (updateP * Time.deltaTime * dashSpeed);
            yield return new WaitForEndOfFrame();
        }
        inDash = false;
        state.CanShoot = true;
        state.Invulnerable = false;
        yield return null;
    }

    private void DashHandler(InputAction.CallbackContext callback)
    {
        if ((Time.time - lastDash) < dashDelay) return;
        lastDash = Time.time;
        lastMoveDir = move.ReadValue<Vector2>();
        StartCoroutine(DashRoutine());
    }

    private void InitializeDash(InputAction action)
    {
        dash = action;
        //dash.Enable();
        dash.performed += DashHandler;
    }

    private void InitializeMove(InputAction action)
    {
        move = action;
        //move.Enable();
    }
}
