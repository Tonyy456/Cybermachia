using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordePlayer : MonoBehaviour, IDamageable
{
    [SerializeField] private bool EnableInput = true;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;

    private int currentHealth = 0;
    public void Start()
    {
        var comps = GetComponents<PlayerInputScript>();
        foreach(var comp in comps)
        {
            comp.EnableInput();
        }
        currentHealth = maxHealth;
        if (healthBar) healthBar.value = 1;
    }

    public bool TryDamage(int damage)
    {
        currentHealth -= damage;
        if(healthBar) healthBar.value = currentHealth / (float)maxHealth;
        return true;
    }
}
