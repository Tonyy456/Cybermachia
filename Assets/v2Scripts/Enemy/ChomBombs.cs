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

    [Header("Raycast Obstacle Detection")]
    [SerializeField] private bool drawRays;
    [SerializeField] private int numRays;
    [SerializeField] private float detectionRadians;
    [SerializeField] private float rayDistance;  

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private CardinalDirection movingDirection;
    private List<GameObject> players;

    public void Update()
    {
        if (!AttemptAvoidance())
        {
            MoveToPlayer(testTarget);
        }
        UpdateAnimation();
    }

    private bool AttemptAvoidance()
    {
        int hitCount = 0;

        for(int rn = 0; rn < numRays; rn++)
        {
            float theta = -detectionRadians / 2 + (rn * detectionRadians / (numRays - 1));
            Vector2 ray = RotateVector(this.rb.velocity, theta) * rayDistance;
            
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, ray, rayDistance);
            if(hit.distance < rayDistance)
            {
                hitCount++;
                if (drawRays) Debug.DrawRay(this.transform.position, ray, Color.red);
            } else
            {
                if (drawRays) Debug.DrawRay(this.transform.position, ray, Color.green);
            }
        }
        if(hitCount > 0)
        {
            return true;
        }
        return false;
    }

    private void MoveToPlayer(GameObject player)
    {
        Vector2 moveTo = player.transform.position - this.transform.position;
        if (moveTo.magnitude < goalDistanceFromTarget)
        {
            rb.velocity = Vector2.zero;
        } 
        else
        {
            rb.velocity = moveTo.normalized * movementSpeed;
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
