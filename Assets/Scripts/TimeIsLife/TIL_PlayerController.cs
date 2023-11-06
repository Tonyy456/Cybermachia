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

    void Start()
    {
        if(enableInput)
        {
            EnableAllInput();
        }
        if (enableHealthTick) health.Enable();
    }

    public void PlayerRespawn()
    {
        health.Reset();
        health.Enable();
        EnableAllInput(true);
    }

    public void PlayerDie(float deadTime)
    {
        EnableAllInput(false);
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
