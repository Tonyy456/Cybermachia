using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class v2_AttackManager : MonoBehaviour, PlayerInputScript
{
    [SerializeField] private bool usingRenderTextureAsCamera = false;
    [SerializeField] private string RenderTextureCameraTag = "";
    [SerializeField] private RenderTexture texture;
    [SerializeField] private int maxAmmoCount = 20;
    [SerializeField] private float fireDelay = 0.1f;
    [SerializeField] private GameObject prefab;

    public bool Enabled { get; set; } = false;
    public UnityEvent onAttack;

    private float lastFire;
    private bool isKeyboard = false;
    private PlayerInput input;
    private InputAction attack;
    private InputAction aim;

    private int ammo;
    private Camera renderCam;
    public int Ammo
    {
        get
        {
            return ammo;
        }
        private set
        {
            ammo = value;
        }
    }

    public void Awake()
    {
        Ammo = maxAmmoCount;
        lastFire = Time.time - 100f;


    }

    public void Start()
    {
        
        input = this.GetComponent<PlayerInput>();
        isKeyboard = input.currentControlScheme.Contains("Keyboard");
        InitializeAttack(input.currentActionMap.FindAction("Attack"));
        InitializeAim(input.currentActionMap.FindAction("Aim"));
        if (usingRenderTextureAsCamera)
        {
            var cams = Camera.allCameras;
            foreach(var cam in cams)
            {
                if (cam.tag == RenderTextureCameraTag)
                {
                    renderCam = cam;
                    break;
                }
            }
        }
        else
        {
            renderCam = Camera.main;
        }
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
        if (Ammo <= 0) return;
        if ((Time.time - lastFire) < fireDelay) return;

        //get aim dir
        Vector2 aimDir = aim.ReadValue<Vector2>();
        if (aimDir.magnitude <= 0.1) return;
        
        if(isKeyboard && usingRenderTextureAsCamera)
        {
            GameObject renderTextureObject = GameObject.FindGameObjectWithTag("RenderTexture");
            Canvas canvas = renderTextureObject.GetComponentInParent<Canvas>();
            RectTransform gameObjectRectTransform = renderTextureObject.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObjectRectTransform, aimDir, Camera.main, out Vector2 localPoint);
            Rect rect = gameObjectRectTransform.rect;
            Vector2 normalizedOnRenderTexture = (localPoint / rect.size) + new Vector2(0.5f, 0.5f);
            Vector3 pixelCoord = normalizedOnRenderTexture * new Vector2(texture.width, texture.height);
            Ray ray = renderCam.ScreenPointToRay(pixelCoord);
            // Example: Cast a ray and get the world point
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPoint = hit.point;

                // Now 'worldPoint' contains the world coordinates in the scene
                Debug.Log("World Point: " + worldPoint);
                aimDir = worldPoint - this.transform.position;
            }
            Vector2 renderCamScreenPoint = normalizedOnRenderTexture * new Vector2(renderCam.pixelWidth, renderCam.pixelHeight);
            Debug.Log($"{aimDir} --> {renderCamScreenPoint}");

            aimDir = renderCam.ScreenToWorldPoint(renderCamScreenPoint) - this.transform.position;
        }
        else if (isKeyboard)
        {
            aimDir = renderCam.ScreenToWorldPoint(aimDir) - this.transform.position;
        }
        lastFire = Time.time;

        Ammo--;
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

    public void ResetAmmo()
    {
        Ammo = maxAmmoCount;
    }
}
