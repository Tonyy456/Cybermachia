using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAvoidance : MonoBehaviour
{
    [SerializeField] private VisionModule visionModule;
    [SerializeField] private Rigidbody2D rb;

    [Header("Avoidance")]
    [SerializeField] private float obstacleAvoidDistance;
    [SerializeField] private float boidAvoidDistance;
    [SerializeField] private float boidAvoidanceFactor;
    [SerializeField] private float obstacleAvoidanceFactor;

    public void Update()
    {
        PerformAvoidance();
    }

    public void PerformAvoidance()
    {
        Vector2 playerPos = (Vector2)this.transform.position; 
        foreach (var boid in visionModule.boids)
        {
            // find exact point obstacle is hit.
            Collider2D colliderB = boid.GetComponent<BoxCollider2D>();
            Collider2D colliderA = this.GetComponent<BoxCollider2D>();
            ColliderDistance2D dst = Physics2D.Distance(colliderA, colliderB);
            Debug.Log(dst.isValid);
            Vector2 dv = (dst.pointA - dst.pointB);
            if (dv.magnitude < 0.01f)
            {
                dv = (playerPos - (Vector2)boid.transform.position);
                rb.velocity += (dv.normalized * Time.deltaTime * boidAvoidanceFactor);
                continue;
            }
            if (dv.magnitude > boidAvoidDistance)
            {
                Debug.DrawRay(dst.pointA, -1 * dv, Color.blue);
                continue;
            }
            else
            {
                Debug.DrawRay(dst.pointA, -1 * dv, Color.magenta);
                rb.velocity += (dv.normalized * Time.deltaTime * boidAvoidanceFactor);
            }
        }
        foreach (var obstacle in visionModule.obstacles)
        {
            // find exact point obstacle is hit.
            Collider2D colliderB = obstacle.GetComponent<BoxCollider2D>();
            Collider2D colliderA = this.GetComponent<BoxCollider2D>();
            ColliderDistance2D dst = Physics2D.Distance(colliderA, colliderB);
            Debug.Log(dst.isValid);
            Vector2 dv = (dst.pointA - dst.pointB);
            if (dv.magnitude < 0.01f) {
                dv = (playerPos - (Vector2)obstacle.transform.position);
                rb.velocity += (dv.normalized * Time.deltaTime * obstacleAvoidanceFactor);
                Debug.DrawRay(playerPos, -1 * dv, Color.red);
                continue;
            }
            if (dv.magnitude > obstacleAvoidDistance) 
            {
                Debug.DrawRay(dst.pointA, -1 * dv, Color.green);
                continue;
            }
            else 
            {
                Debug.DrawRay(dst.pointA, -1 * dv, Color.red);
                rb.velocity += (dv.normalized * Time.deltaTime * obstacleAvoidanceFactor);
            }
        }
    }
}
