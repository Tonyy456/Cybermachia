using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIL_HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private AIL_UIHealth ui;

    public bool Enabled { get; private set; } = false;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            ui.UpdateUI(health, maxHealth);
        }
    }

    public void Enable(bool on = true)
    {
        Enabled = on;
    }

    public void Reset()
    {
        Health = maxHealth;
    }
}
