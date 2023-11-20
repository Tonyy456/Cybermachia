using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidCohesion : MonoBehaviour
{
    [SerializeField] private VisionModule visionModule;
    [SerializeField] private Rigidbody2D rb;

    [Header("Cohesion")]
    [SerializeField] private float cohesionFactor;

    public void Update()
    {
        PerformCohesion();
    }

    public void PerformCohesion()
    {
        Vector3 avgPos = Vector3.zero;
        foreach (var boid in visionModule.boids)
        {
            avgPos += boid.transform.position;
        }
        rb.velocity += (Time.deltaTime * cohesionFactor) * ((Vector2)avgPos - (Vector2)this.transform.position);
    }
}
