using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAlignmentModule : MonoBehaviour 
{
    [SerializeField] private VisionModule visionModule;
    [SerializeField] private Rigidbody2D rb;

    [Header("Alignment")]
    [SerializeField] private float alignmentFactor;

    public void Update()
    {
        PerformAllignment();
    }

    public void PerformAllignment()
    {
        foreach(var boid in visionModule.boids)
        {
            Rigidbody2D rb = boid.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            Vector2 moveDir = rb.velocity;
            if(moveDir.magnitude > 0.1f)
                this.rb.velocity += (moveDir * Time.deltaTime * alignmentFactor);
        }
    }
}
