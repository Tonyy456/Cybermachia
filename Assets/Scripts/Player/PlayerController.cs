using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private PlayerAttackController attack;
    [SerializeField] private PlayerMovementController move;
    [SerializeField] private PlayerState state;

    [SerializeField] private GameObject turnOffOnDeath;

    [SerializeField] private GameObject textPrefab;

    private PlayerInput input;

    public void OnValidate()
    {
        if (box == null) box = this.GetComponent<BoxCollider2D>();
        if (attack == null) attack = this.GetComponent<PlayerAttackController>();
        if (move == null) move = this.GetComponent<PlayerMovementController>();
        if (state == null) state = this.GetComponent<PlayerState>();
    }

    public void Start()
    {
        input = this.GetComponent<PlayerInput>();
    }

    public void PausePlayer()
    {
        foreach (var i in input.currentActionMap.actions) i.Disable();
        state.Invulnerable = true;
        if (box) box.enabled = false;

    }

    public void EnablePlayer()
    {
        foreach (var i in input.currentActionMap.actions) i.Enable();
        state.Invulnerable = false;
        if (box) box.enabled = true;
    }

    public void DamagePlayer(int damage)
    {
        if (state.TryDamage(damage))
        {
            var textPfb = GameObject.Instantiate(textPrefab);
            var behavior = textPfb.GetComponent<TextPopUpBehaviour>();
            behavior.Initialize($"-{damage}", this.transform.position);
            if(state.Health <= 0)
            {
                HandleDeath();
            }
        } 
    }

    public void HandleDeath()
    {
        //reset health.
        turnOffOnDeath.SetActive(false);
        state.ResetHealth();
        PausePlayer();
    }

    public void HandleRespawn(Vector2 newLocation)
    {
        turnOffOnDeath.SetActive(true);
        EnablePlayer();
        // to implement
    }

}
