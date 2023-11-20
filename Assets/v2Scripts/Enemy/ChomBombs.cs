using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChomBombs : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float passiveSpeed = 1f;
    [SerializeField] private float goalDistanceToTarget;
    [SerializeField] private BoidTargeting targetModule;


    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private VisionModule vision;
    [SerializeField] private ChomAnimController chomAnimController;
    [SerializeField] private float deathAnimationLength = 1f;

    private bool queuedDeath = false;

    public void Update()
    {
    }

    public IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathAnimationLength);
        GameObject.Destroy(this.gameObject);
        yield return null;
    }

    public void LateUpdate()
    {
        if (queuedDeath)
        {
            rb.velocity = Vector2.zero; 
            return;
        }
        if (vision.targets.Count == 0)
        { // I aint wandering! too much effort!
            rb.velocity = Vector2.zero;
        }
        else if (closeToTarget())
        { // I am satisfied, made it where I belong.
            queuedDeath = true;
            chomAnimController.PlayDeath();
            StartCoroutine(DeathRoutine());
            rb.velocity = Vector2.zero;
        }
        else if(rb.velocity.magnitude > movementSpeed)
        { // zoom! slow down buddy!
            rb.velocity = (rb.velocity / rb.velocity.magnitude) * movementSpeed;
        }

        if(!queuedDeath) chomAnimController.UpdateAnimation();



    }

    private bool closeToTarget()
    {
        if (targetModule == null) return false;
        return targetModule.lastPointCheck.distance < goalDistanceToTarget;
    }
}
