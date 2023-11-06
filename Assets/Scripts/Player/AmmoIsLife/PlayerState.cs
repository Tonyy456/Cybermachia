using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private bool invincible;
    [SerializeField] private GameObject textPopUpPrefab;

    public int Health { get; private set; }
    public bool Invulnerable { get; set; } = false;
    public bool CanShoot { get; set; } = true;
    public bool Paused { get; set; } = false;

    public void Awake()
    {
        Health = maxHealth;
    }

    public void ResetHealth()
    {
        Health = maxHealth;
    }

    public bool TryDamage(int damage)
    {
        if (invincible || Invulnerable) return false;
        Health -= Mathf.Clamp(damage, 0, int.MaxValue);
        return true;
    }
}
