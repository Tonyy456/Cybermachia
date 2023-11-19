using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardinalDirection
{
    Up,
    Down,
    Left,
    Right
}
public class ChomBombs : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float movementSpeed;

    [Header("Target AI")]
    [SerializeField] private GameObject testTarget;
    [SerializeField] private float goalDistanceFromTarget = 0.2f;
    [SerializeField] private float chaseFactor;

    [Header("Raycast Obstacle Detection")]
    [SerializeField] private bool drawRays;
    [SerializeField] private int numRays;
    [SerializeField] private float detectionRadians;
    [SerializeField] private float rayDistance;  
    [SerializeField] private float avoidanceFactor;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private CardinalDirection movingDirection;
    private List<GameObject> players;

    public void Update()
    {
        AttemptAvoidance();
        SteerToPlayer(testTarget);
        CapSpeed();
        UpdateAnimation();
    }

    private void CapSpeed()
    {
        Vector2 v = rb.velocity;
        float speed = v.magnitude;
        if(speed > movementSpeed)
        {
            v = (v / speed) * movementSpeed;
        }
        rb.velocity = v;      
    }

    private bool AttemptAvoidance()
    {
        int numRaysHit = 0;
        Vector2 dv = Vector2.zero;
        for (int rn = 0; rn < numRays; rn++)
        {
            float theta = -detectionRadians / 2 + (rn * detectionRadians / (numRays - 1));
            Vector2 ray = RotateVector(this.rb.velocity, theta) * rayDistance;
            
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, ray, rayDistance);
            if(hit.collider == null)
            {
                if (drawRays) Debug.DrawRay(this.transform.position, ray, Color.red);
            } else
            {
                numRaysHit++;
                //dv += ((Vector2)this.transform.position - hit.point) * ((float)Math.PI - Math.Abs(theta)) * (rayDistance - hit.distance);              
                if (drawRays) Debug.DrawRay(this.transform.position, ray, Color.green);
            }
        }
        if(numRaysHit > 0)
        {
            rb.velocity += dv * avoidanceFactor * movementSpeed * Time.deltaTime;
            return true;
        }
        return false;
    }

    private void SteerToPlayer(GameObject player)
    {
        Vector2 moveTo = player.transform.position - this.transform.position;
        if (moveTo.magnitude < goalDistanceFromTarget)
        {
            rb.velocity = Vector2.zero;
        } 
        else
        {
            rb.velocity += moveTo.normalized * movementSpeed * chaseFactor * Time.deltaTime;
        }      
    }

    private void UpdateAnimation()
    {
        Vector2 dir = rb.velocity;
        if(dir.x < -0.1)
        {
            movingDirection = CardinalDirection.Left;
        }
        else if (dir.x > 0.1)
        {
            movingDirection= CardinalDirection.Right;
        }
        else if (dir.y > 0.01)
        {
            movingDirection = CardinalDirection.Up;
        } 
        else if (dir.y < -0.01)
        {
            movingDirection = CardinalDirection.Down;
        }
        else
        { // not moving any direction
            
        }

        if(dir.magnitude > 0.01)
        {
            string animationName = $"Run{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
            animator.Play(animationName);
        } else
        {
            string animationName = $"Idle{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
            animator.Play(animationName);
        }
            
    }

    private Vector2 RotateVector(Vector2 vec, float angle)
    {
        Vector2 result = Vector2.zero;
        vec.Normalize();
        result.x = Mathf.Cos(angle) * vec.x + -1 * Mathf.Sin(angle) * vec.y;
        result.y = Mathf.Sin(angle) * vec.x + Mathf.Cos(angle) * vec.y;
        return result.normalized;
    }
}
