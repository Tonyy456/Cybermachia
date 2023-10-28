using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private bool invincible;

    public int Health { get; private set; }
    public bool Invulnerable { get; set; } = false;
    public bool CanShoot { get; set; } = true;

    public void Awake()
    {
        Health = maxHealth;
    }

    public void Damage(int damage)
    {
        if (invincible || Invulnerable) return;

        Health -= Mathf.Clamp(damage, 0, int.MaxValue);
    }

    public void Heal(int effect)
    {
        Health += Mathf.Clamp(effect, 0, int.MaxValue);
    }
}
