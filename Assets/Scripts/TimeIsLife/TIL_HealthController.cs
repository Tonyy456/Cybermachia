using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TIL_HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float minHealth;
    [SerializeField] private float iFrameTime = 1f;
    [SerializeField] private TIL_UIHealth ui;

    [SerializeField] private float health;
    public float Health {
        get
        {
            return health;
        }
        private set
        {
            health = value;
            ui.UpdateUI(health, maxHealth);
        }
    }
    public bool Enabled { get; set; } = false;
    public bool InDamageCooldown { get; set; } = false;
    public UnityEvent OnHealthOut;

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    public void Awake()
    {
        Health = maxHealth;
    }

    public void UpdateHealth(float diff)
    {
        if (!Enabled || Health <= minHealth) return;
        if (InDamageCooldown) return;
        Health += diff;
        TextPopupManager manager2 = GameObject.FindObjectOfType<TextPopupManager>();
        manager2.HandlePopup($"-{diff.ToString("F0")}", this.transform.position);
        InDamageCooldown = true;
        StartCoroutine(EnableInTime(iFrameTime));
        if (Health < minHealth) OnHealthOut?.Invoke();
    }

    public void Reset()
    {
        Health = maxHealth;
    }

    public void Update()
    {
        if (!Enabled || Health <= minHealth) return;
        Health -= Time.deltaTime;
        if (Health < minHealth) OnHealthOut?.Invoke();
    }

    public IEnumerator EnableInTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        InDamageCooldown = false;
    }
}
