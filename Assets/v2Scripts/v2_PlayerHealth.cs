using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class v2_PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int defaultHealth = 10;
    [SerializeField] private bool invincible = false;
    [SerializeField] public UnityAction onHealthDepleated;

    private int health;
    public int Health { 
        get 
        { 
            return health;  
        } 
        private set
        {
            health = value;
        }
    }

    public void Reset()
    {
        health = defaultHealth;
    }

    public void EnableDamage(bool status)
    {
        invincible = status;
    }

    public bool TryDamage(int damage)
    {
        if (invincible) return false;
        Health -= damage;
        return true;
    }
}
