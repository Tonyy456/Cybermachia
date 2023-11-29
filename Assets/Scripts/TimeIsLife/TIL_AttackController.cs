using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class TIL_AttackController : MonoBehaviour, PlayerInputScript
{
    [Header("Needs a TIL_BulletManager")]
    [SerializeField] private float fireDelay = 0.1f;

    [SerializeField] private bool usingRenderTextureAsCamera = false;
    [SerializeField] private string RenderTextureCameraTag = "";
    [SerializeField] private RenderTexture texture;

    public bool Enabled { get; set; } = false;

    private float lastFire;
    private bool isKeyboard = false;
    private Camera renderCam;
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
        if (usingRenderTextureAsCamera)
        {
            var cams = Camera.allCameras;
            foreach (var cam in cams)
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
        if ((Time.time - lastFire) < fireDelay) return;

        //get aim dir
        Vector2 aimDir = aim.ReadValue<Vector2>();
        if (aimDir.magnitude <= 0.1) return;
        if (isKeyboard && usingRenderTextureAsCamera)
        {
            GameObject renderTextureObject = GameObject.FindGameObjectWithTag("RenderTexture");
            Canvas canvas = renderTextureObject.GetComponentInParent<Canvas>();
            RectTransform gameObjectRectTransform = renderTextureObject.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObjectRectTransform, aimDir, Camera.main, out Vector2 localPoint);
            Rect rect = gameObjectRectTransform.rect;
            Vector2 normalizedOnRenderTexture = (localPoint / rect.size) + new Vector2(0.5f, 0.5f);
            Vector2 renderCamScreenPoint = normalizedOnRenderTexture * new Vector2(renderCam.pixelWidth, renderCam.pixelHeight);
            aimDir = renderCam.ScreenToWorldPoint(renderCamScreenPoint) - this.transform.position;
        }
        else if (isKeyboard)
        {
            aimDir = renderCam.ScreenToWorldPoint(aimDir) - this.transform.position;
        }
        lastFire = Time.time;

        TIL_BulletManager manager = GameObject.FindObjectOfType<TIL_BulletManager>();
        manager?.FireBullet(this.gameObject, aimDir, Vector2.zero);
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
