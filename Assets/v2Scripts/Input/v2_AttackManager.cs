using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class v2_AttackManager : MonoBehaviour, PlayerInputScript
{
    [SerializeField] private float fireDelay = 0.1f;
    [SerializeField] private GameObject prefab;

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
        GameObject bullet = GameObject.Instantiate(prefab);
        var script = bullet.GetComponent<v2_bullet>();
        script.Initialize(this.gameObject, aimDir);
        bullet.gameObject.transform.position = this.transform.position;
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
