using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class TargetShooterPlayer : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerInput input;

    [Header("Option Render Texture Setup")]
    [SerializeField] private bool usingRenderTextureAsCamera = false;
    [SerializeField] private string RenderTextureCameraTag = "";
    [SerializeField] private RenderTexture texture;

    [Header("Shooting")]
    [SerializeField] private bool regenerateByDefault = false;
    [SerializeField] private float delayBetweenShots = 0.5f;
    [SerializeField] private float bulletRegenerationTime;
    [SerializeField] private TargetShooterBullet bulletPrefab;
    [SerializeField] private bool allowedToShootByDefault = false;
    [SerializeField] private UnityEvent onShoot;

    [Header("UI")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animationToPlay;
    [SerializeField] private Transform BulletUIParent;


    private bool allowedToShoot = false;
    public bool AllowedToShoot
    {
        get => allowedToShoot;
        set
        {
            allowedToShoot = value;
            if (shootAction == null) return;
            if (allowedToShoot) shootAction.Enable();
            else shootAction.Disable();
        }
    }
    private float lastShotTime = float.MinValue;

    private InputAction shootAction;
    private InputAction aim;
    private bool isKeyboard = false;
    private Camera renderCam;

    private int maxBullets = 0;
    private int bullets = 0;
    public int BulletCount
    {
        get
        {
            return bullets;
        }
        private set
        {
            bullets = Mathf.Clamp(value, 0, maxBullets);
            for(int i = 0; i < BulletUIParent.childCount; i++)
            {
                BulletUIParent.GetChild(i).gameObject.SetActive(i < bullets);
            }
        }
    }

    private bool regenBullets = false;
    private IEnumerator bulletRegenRoutine = null;
    public bool RegenerateBullets
    {
        get
        {
            return regenBullets;
        }
        set
        {
            regenBullets = value;          
            if(regenBullets)
            {
                if(bulletRegenRoutine == null)
                {
                    StopAllCoroutines();
                    var bulletRegenRoutine = BulletRegenerationRoutine();
                    StartCoroutine(bulletRegenRoutine);
                }
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    public void Start()
    {
        shootAction = input.currentActionMap.FindAction("Attack");
        shootAction.performed += OnShoot;
        aim = input.currentActionMap.FindAction("Aim");
        maxBullets = BulletUIParent.childCount;
        bullets = maxBullets;
        if (animator) animator.Play(animationToPlay);
        if (regenerateByDefault) RegenerateBullets = true;
        if (allowedToShootByDefault) AllowedToShoot = true;
        isKeyboard = input.currentControlScheme.Contains("Keyboard");
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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if ((Time.time - lastShotTime) < delayBetweenShots) return;
        if (BulletCount <= 0 || !AllowedToShoot) return;
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
        lastShotTime = Time.time;
        onShoot?.Invoke();
        BulletCount -= 1;
        SoundEffectManager.TryPlay("shoot1");

        TargetShooterBullet go = GameObject.Instantiate(bulletPrefab);
        go.gameObject.transform.position = this.gameObject.transform.position;
        go.Init(input, aimDir);
    }

    public IEnumerator BulletRegenerationRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(bulletRegenerationTime);
            BulletCount += 1;
        }
    }
}
