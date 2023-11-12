using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIL_PlayerController : MonoBehaviour
{
    [SerializeField] private bool enableInput = false;
    [SerializeField] private bool enableHealthTick = true;
    [SerializeField] private TIL_MovementController movement;
    [SerializeField] private TIL_HealthController health;
    [SerializeField] private float stayDeadForSeconds = 1f;
    private SpawnHandler handler;

    public float Health 
    { 
        get
        {
            return health.Health;
        } 
    }
    public bool Damageable
    {
        get
        {
            return !health.InDamageCooldown && health.Enabled;
        }
    }

    void Start()
    {
        handler = GameObject.FindObjectOfType<SpawnHandler>();
        if (handler) {
            Vector2 spawn = handler.GetSpawnLocation();
            this.transform.position = new Vector3(spawn.x, spawn.y, this.transform.position.z);
        } 
        if (enableInput)
        {
            EnableAllInput();
        }
        if (enableHealthTick) EnableHealthTick();
    }

    public void EnableHealthTick()
    {
        health.Enable();
    }

    public void PlayerRespawn()
    {
        health.Reset();
        EnableHealthTick();
        EnableAllInput(true);
        if (handler)
        {
            Vector2 spawn = handler.GetSpawnLocation();
            this.transform.position = new Vector3(spawn.x, spawn.y, this.transform.position.z);
        }
    }

    public void PlayerDie(float deadTime)
    {
        EnableAllInput(false);
        health.Disable();
        StartCoroutine(RespawnInTime(stayDeadForSeconds));
    }

    public IEnumerator RespawnInTime(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerRespawn();
    }

    public void UpdateHealth(float difference)
    {
        health.UpdateHealth(difference);
    }

    // just simple lol. scales nicely for now.
    public void EnableAllInput(bool enabled = true)
    {
        PlayerInputScript[] scripts = this.GetComponentsInChildren<PlayerInputScript>();
        foreach(var script in scripts)
        {
            if (enabled) script.EnableInput();
            else script.DisableInput();
        }
    }
}
