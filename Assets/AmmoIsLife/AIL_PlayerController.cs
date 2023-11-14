using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIL_PlayerController : MonoBehaviour
{
    [SerializeField] private bool enableInput = false;
    [SerializeField] private bool enableHealthLoss = true;
    [SerializeField] private bool useSpawnPoints = true;
    [SerializeField] private TIL_MovementController movement;
    [SerializeField] private AIL_HealthController health;
    [SerializeField] private float stayDeadForSeconds = 1f;
    private SpawnHandler handler;

    //public float Health
    //{
    //    get
    //    {
    //        return health.Health;
    //    }
    //}
    //public bool Damageable
    //{
    //    get
    //    {
    //        return !health.InDamageCooldown && health.Enabled;
    //    }
    //}

    void Start()
    {
        health.Reset();
        handler = GameObject.FindObjectOfType<SpawnHandler>();
        if (useSpawnPoints && handler)
        {
            Vector2 spawn = handler.GetSpawnLocation();
            this.transform.position = new Vector3(spawn.x, spawn.y, this.transform.position.z);
        }
        if (enableInput)
        {
            EnableAllInput();
        }
        if (enableHealthLoss) EnableHealth();
    }

    public void EnableHealth()
    {
        health.Enable();
    }

    public void PlayerRespawn()
    {
        health.Reset();
        EnableHealth();
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
        health.Enable(false);
        StartCoroutine(RespawnInTime(stayDeadForSeconds));
    }

    public IEnumerator RespawnInTime(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerRespawn();
    }

    public void UpdateHealth(int difference)
    {
        health.Health += difference;
    }

    // just simple lol. scales nicely for now.
    public void EnableAllInput(bool enabled = true)
    {
        PlayerInputScript[] scripts = this.GetComponentsInChildren<PlayerInputScript>();
        foreach (var script in scripts)
        {
            if (enabled) script.EnableInput();
            else script.DisableInput();
        }
    }
}
