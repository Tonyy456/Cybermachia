using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityClamp : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    private Rigidbody2D rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LateUpdate()
    {
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = (rb.velocity / rb.velocity.magnitude) * maxSpeed;
        }
    }
}
