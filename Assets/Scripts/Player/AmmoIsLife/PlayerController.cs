using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private PlayerAttackController attack;
    [SerializeField] private PlayerMovementController move;
    [SerializeField] private PlayerState state;

    [SerializeField] private GameObject turnOffOnDeath;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private SpriteRenderer playerImage;
    [SerializeField] private bool keepDisabled;

    private PlayerInput input;
    public PlayerAmmoUIController AmmoUIController { get; set; }

    public void InitializePlayerData(PlayerAmmoUIController controller, Color playerColor)
    {
        AmmoUIController = controller;
        AmmoUIController.SetAmmo(state.Health);
        AmmoUIController.gameObject.SetActive(true);
        AmmoUIController.SetColor(playerColor);
        playerImage.color = playerColor;
    }


    public void OnValidate()
    {
        if (box == null) box = this.GetComponent<BoxCollider2D>();
        if (attack == null) attack = this.GetComponent<PlayerAttackController>();
        if (move == null) move = this.GetComponent<PlayerMovementController>();
        if (state == null) state = this.GetComponent<PlayerState>();
    }

    public void Awake()
    {
        input = this.GetComponent<PlayerInput>();
    }


    public void Start()
    {
        if (AmmoUIController) AmmoUIController.SetAmmo(state.Health);
        if(state.Paused || keepDisabled)
        { PausePlayer(); } else{EnablePlayer(); // ensure player movement is enabled
        }
    }

    public void PausePlayer()
    {
        state.Paused = true;
        if (input == null) input = this.GetComponent<PlayerInput>();
        foreach (var i in input.currentActionMap.actions) i.Disable();
        state.Invulnerable = true;
        if (box) box.enabled = false;
    }

    public void EnablePlayer()
    {
        state.Paused = false;
        if (input == null) input = this.GetComponent<PlayerInput>();
        foreach (var i in input.currentActionMap.actions) i.Enable();
        state.Invulnerable = false;
        if (box) box.enabled = true;
    }

    public void DamagePlayer(int damage)
    {
        if (state.TryDamage(damage))
        {
            AmmoUIController.SetAmmo(state.Health);
            var textPfb = GameObject.Instantiate(textPrefab);
            var behavior = textPfb.GetComponent<TextPopUpBehaviour>();
            behavior.Initialize($"-{damage}", this.transform.position, Color.red);
            if(state.Health <= 0)
            {
                HandleDeath();
            }
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var blt = collision.GetComponent<BulletBehavior>();
        if(blt != null && blt.spawnedFrom != this.gameObject)
        {
            int damage = blt.damage;
            if (state.Health <= damage)
            {
                ScoreHandler handler = GameObject.FindObjectOfType<ScoreHandler>();
                int playerIndex = this.GetComponent<PlayerInput>().playerIndex;
                handler.PlayerKilled(playerIndex, blt.spawnedFrom.GetComponent<PlayerInput>().playerIndex);
            }
            this.DamagePlayer(damage);
            blt.Explode();
        }
    }

    public void HandleDeath()
    {
        //reset health.
        turnOffOnDeath.SetActive(false);
        PausePlayer();
        StartCoroutine(WaitToRespawn(3f));
    }

    public IEnumerator WaitToRespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleRespawn();
    }

    public void HandleRespawn()
    {
        turnOffOnDeath.SetActive(true);
        state.ResetHealth();
        AmmoUIController.SetAmmo(state.Health);
        EnablePlayer();
        var item = GameObject.FindObjectOfType<PlayerSpawnHandler>();
        item.SpawnPlayer(input);
    }
}
