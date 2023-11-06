using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(PlayerState))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("ATTACK")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireDelay = 0.1f;

    //Control
    public UnityEvent OnAmmoChange { get; set; }
    private float lastFire;
    private bool isKeyboard = false;

    //Input Actions
    private PlayerInput input;
    private InputAction attack;
    private InputAction aim;

    //State
    private PlayerState state;
    private PlayerController controller;

    public void Awake()
    {
        lastFire = Time.time - 100f;
    }

    public void Start()
    {
        state = this.GetComponent<PlayerState>();
        input = this.GetComponent<PlayerInput>();
        controller = this.GetComponent<PlayerController>();
        isKeyboard = input.currentControlScheme.Contains("Keyboard");
        InitializeAttack(input.currentActionMap.FindAction("Attack"));
        InitializeAim(input.currentActionMap.FindAction("Aim"));
    }

    private void ShootHandler(InputAction.CallbackContext callback)
    {
        if (!state.CanShoot) return;
        if ((Time.time - lastFire) < fireDelay) return;

        //get aim dir
        Vector2 aimDir = aim.ReadValue<Vector2>();
        if (aimDir.magnitude <= 0.1) return;
        if (isKeyboard) aimDir = Camera.main.ScreenToWorldPoint(aimDir) - this.transform.position;

        lastFire = Time.time;

        //spawn projectile, set position, set rotation, get script
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = this.transform.position; //modify later?
        bullet.GetComponent<BulletBehavior>().Initialize(aimDir, this.gameObject);
        controller.DamagePlayer(1);
    }

    private void InitializeAim(InputAction action)
    {
        aim = action;
    }

    private void InitializeAttack(InputAction action)
    {
        attack = action;
        attack.performed += ShootHandler;
    }
}
