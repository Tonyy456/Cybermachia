using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    //Movement Handling
    [Header("MOVEMENT")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashLength = 1f;
    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private bool canMoveInDash = false;

    //Control
    private bool inDash = false;
    private Vector2 lastMoveDir = Vector2.zero;
    private float lastDash;

    //Input Actions
    private PlayerInput input;
    private InputAction move;
    private InputAction dash;
    private InputAction a1;
    private InputAction attack;

    public void Awake()
    {
        lastDash = Time.time - 1f;
    }

    public void Start()
    {
        input = this.GetComponent<PlayerInput>();
        InitializeMove(input.currentActionMap.FindAction("Move"));
        InitializeActionOne(input.currentActionMap.FindAction("ActionOne"));
        InitializeActionTwo(input.currentActionMap.FindAction("ActionTwo"));
        InitializeAttack(input.currentActionMap.FindAction("Attack"));
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
        float current = Time.time;
        while((Time.time - current) < dashLength)
        {
            Vector2 dp = lastMoveDir;
            Vector3 updateP = new Vector3(dp.x, dp.y, 0);
            this.transform.position += (updateP * Time.deltaTime * dashSpeed);
            yield return new WaitForEndOfFrame();
        }
        inDash = false;
        yield return null;
    }

    private void DashHandler(InputAction.CallbackContext callback)
    {
        if ((Time.time - lastDash) < dashDelay) return;
        lastDash = Time.time;
        lastMoveDir = move.ReadValue<Vector2>();
        StartCoroutine(DashRoutine());
    }

    //MOVE
    private void InitializeMove(InputAction action)
    {
        move = action;
        move.Enable();
    }

    // DASH?
    private void InitializeActionOne(InputAction action)
    {
        a1 = action;
        action.Enable();
    }

    private void InitializeActionTwo(InputAction action)
    {
        dash = action;
        dash.Enable();
        dash.performed += DashHandler;
    }

    //SHOOT? 
    private void InitializeAttack(InputAction action)
    {
        attack = action;
        action.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
        a1.Disable();
        attack.Disable();
    }
}
