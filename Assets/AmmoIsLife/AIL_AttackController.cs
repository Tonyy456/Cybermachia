using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Events;

public class AIL_AttackController : MonoBehaviour, PlayerInputScript
{
    [Header("Needs a AIL_BulletManager")]
    [SerializeField] private float fireDelay = 0.1f;

    public bool Enabled { get; set; } = false;
    public UnityEvent onAttack;

    private float lastFire;
    private bool isKeyboard = false;
    private PlayerInput input;
    private InputAction attack;
    private InputAction aim;

    public void Awake()
    {
        lastFire = Time.time - 100f;
    }

    public void Start()
    {
        input = this.GetComponent<PlayerInput>();
        isKeyboard = input.currentControlScheme.Contains("Keyboard");
        InitializeAttack(input.currentActionMap.FindAction("Attack"));
        InitializeAim(input.currentActionMap.FindAction("Aim"));
    }

    public void DisableInput()
    {
        Enabled = false;
        if (attack == null || aim == null) return;
        attack.Disable();
        aim.Disable();
    }

    public void EnableInput()
    {
        Enabled = true;
        if (attack == null || aim == null) return;
        attack.Enable();
        aim.Enable();
    }

    private void ShootHandler(InputAction.CallbackContext callback)
    {
        if ((Time.time - lastFire) < fireDelay) return;

        //get aim dir
        Vector2 aimDir = aim.ReadValue<Vector2>();
        if (aimDir.magnitude <= 0.1) return;
        if (isKeyboard) aimDir = Camera.main.ScreenToWorldPoint(aimDir) - this.transform.position;
        lastFire = Time.time;

        //Bullet Manager
        TIL_BulletManager manager = GameObject.FindObjectOfType<TIL_BulletManager>();
        manager.FireBullet(this.gameObject, aimDir, Vector2.zero);

        onAttack?.Invoke();
    }

    private void InitializeAim(InputAction action)
    {
        aim = action;
        if (Enabled) aim.Enable();
        else aim.Disable();
    }

    private void InitializeAttack(InputAction action)
    {
        attack = action;
        attack.performed += ShootHandler;
        if (Enabled) attack.Enable();
        else attack.Disable();
    }
}
