using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TIL_HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float minHealth;
    [SerializeField] private TIL_UIHealth ui;

    private float health;
    public float Health {
        get
        {
            return health;
        }
        private set
        {
            ui.UpdateUI(health, maxHealth);
            health = value;
        }
    }
    public bool Enabled { get; set; } = false;
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
        Health += diff;
        if (Health < minHealth) OnHealthOut?.Invoke();
    }

    public void Reset()
    {
        Health = maxHealth;
    }

    public void Update()
    {
        Debug.Log(1);
        if (!Enabled || Health <= minHealth) return;
        Debug.Log(2);
        Health -= Time.deltaTime;
        if (Health < minHealth) OnHealthOut?.Invoke();
    }
}
