using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HordePlayer : MonoBehaviour, IDamageable
{
    [SerializeField] private bool EnableInput = true;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private UnityEvent onDead;

    private int currentHealth = 0;
    public int Health {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (healthBar) healthBar.value = (value) / (float)maxHealth;
        }
    }
    public bool Dead { get; private set; }

    public void Start()
    {
        HandleInitialSpawn();

        Health = maxHealth;      
    }

    public bool TryDamage(int damage)
    {
        if (Dead) return false;
        Health -= damage;
        if (Health == 0)
        {
            onDead?.Invoke();
            Kill();
        }
        return true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var collectable = collision.transform.GetComponent<ICollectable>();
        collectable?.Collect(this.gameObject);
    }

    private void EnableChildren(bool enabled = false)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(enabled);
        }
    }

    private void SetImageAlpha(float alpha)
    {
        var images = GetComponentsInChildren<SpriteRenderer>();
        foreach (var image in images)
        {
            if (image)
            {
                var color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
    }

    private void EnableColliders(bool enable = true)
    {
        var colliders = GetComponentsInChildren<Collider2D>();
        foreach(var collider in colliders)
        {
            collider.enabled = enable;
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
    }

    public void Kill()
    {
        Dead = true;
        EnableAllInputs(false);
        EnableChildren(false);
        EnableColliders(false);
    }

    private void HandleInitialSpawn()
    {
        Respawn();
        PlaceOnRespawnPoint();
        EnableAllInputs(EnableInput);
    }

    public void EnableAllInputs(bool enable = true)
    {
        var comps = GetComponents<PlayerInputScript>();
        foreach (var comp in comps)
        {
            if (enable) comp.EnableInput();
            else comp.DisableInput();
        }
    }

    private void PlaceOnRespawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        this.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    public void Respawn()
    {
        if (Dead)
        {
            Dead = false;
            PlaceOnRespawnPoint();
            EnableAllInputs(true);
            EnableColliders(true);
            EnableChildren(true);
            ResetHealth();
        }
    }

    public void ResetHealth()
    {
        Health = maxHealth;
    }

    public void RespawnInSeconds(float seconds)
    {
        StartCoroutine(RespawnRoutine(seconds));
    }

    public IEnumerator RespawnRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Respawn();
    }

}
