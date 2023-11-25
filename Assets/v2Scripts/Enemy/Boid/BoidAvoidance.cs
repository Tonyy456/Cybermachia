using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            if (boid == null || boid.gameObject.IsDestroyed()) continue;
            // find exact point obstacle is hit.
            Collider2D colliderB = boid.GetComponent<BoxCollider2D>();
            Collider2D colliderA = this.GetComponent<BoxCollider2D>();
            ColliderDistance2D dst = Physics2D.Distance(colliderA, colliderB);
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
            if (obstacle == null || obstacle.gameObject.IsDestroyed()) continue;
            Collider2D colliderB = obstacle.GetComponent<Collider2D>();
            Collider2D colliderA = this.GetComponent<BoxCollider2D>();
            if (colliderB == null) continue;
            if (colliderA == null) throw new System.Exception("Boid Collider was null");
            ColliderDistance2D dst = Physics2D.Distance(colliderA, colliderB);
            Vector2 dv = (dst.pointA - dst.pointB);
            if (dv.magnitude <= 0.01f) {
                Transform target = visionModule.targets.First();
                float minDistance = 100000f;
                foreach (var item in visionModule.targets)
                {
                    float dist = ((Vector2)item.transform.position - (Vector2)this.transform.position).magnitude;
                    if (dist < minDistance)
                    {
                        target = item;
                        minDistance = dist;
                    }
                }
                Collider2D colliderB1 = target.GetComponent<BoxCollider2D>();
                Collider2D colliderA1 = this.GetComponent<BoxCollider2D>();
                var lastPointCheck = Physics2D.Distance(colliderA1, colliderB1);
                Vector2 dp = (lastPointCheck.pointB - lastPointCheck.pointA);
                Debug.DrawRay(dst.pointA, -1 * dv.normalized, Color.red);
                Vector2 result = (-1 * dv.normalized * (1/dv.magnitude) * Time.deltaTime * obstacleAvoidanceFactor);
                result = (result.normalized + dp.normalized) * result.magnitude;
                Debug.Log(result.magnitude);
                rb.velocity += result;
                Debug.Log("Ontop of obstacle");
                continue;
            }
            if (dv.magnitude > obstacleAvoidDistance) 
            {
                Debug.DrawRay(dst.pointA, -1 * dv, Color.red);
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
