using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidTargeting : MonoBehaviour
{
    [SerializeField] private VisionModule visionModule;
    [SerializeField] private Rigidbody2D rb;

    [Header("Targeting")]
    [SerializeField] private float targetFactor;
    [SerializeField] private bool normalizeVectors;

    public ColliderDistance2D lastPointCheck { get; private set; }

    public void Update()
    {
        PerformTargeting();
    }

    public void PerformTargeting()
    {
        if (visionModule.targets.Count == 0) return;

        Transform target = visionModule.targets.First();
        float minDistance = 100000f;
        foreach (var item in visionModule.targets)
        {
            float dist = ((Vector2)item.transform.position - (Vector2)this.transform.position).magnitude;
            if (dist < minDistance)
            {
                target = item;
                minDistance= dist;
            }
        }
        Collider2D colliderB = target.GetComponent<BoxCollider2D>();
        Collider2D colliderA = this.GetComponent<BoxCollider2D>();
        lastPointCheck = Physics2D.Distance(colliderA, colliderB);

        Vector2 dp = (lastPointCheck.pointB - lastPointCheck.pointA);
        Debug.DrawRay(lastPointCheck.pointA, dp, Color.cyan);
        if (normalizeVectors) dp = dp.normalized;
        rb.velocity += targetFactor * Time.deltaTime * dp;
    }
}
