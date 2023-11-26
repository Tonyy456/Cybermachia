using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChomBombAgent : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private ChomVisionModule vision;
    [SerializeField] private Animator animator;
    [Range(0f, 1f)]
    [SerializeField] private float idleSpeedThreshold = 0.05f;
    [Range(0f, 90f)]
    [SerializeField] private float upDownAngleThreshold = 45f;
    [SerializeField] private float updateDelay = 1f;

    private NavMeshAgent agent;
    private CardinalDirection movingDirection = CardinalDirection.Down;
    public int Health { get; private set; }
    private bool beingDamaged = false;
    private bool dying = false;

    public void Start()
    {
        Health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(DelayedReupdatingRoutine());
    }

    public void Update()
    {

    }

    public IEnumerator DelayedReupdatingRoutine()
    {
        while (true)
        {
            if (!dying)
            {
                Vector3 target = CurrentTarget();
                target.z = 0;
                agent.destination = target;
                if (!beingDamaged) UpdateAnimator();
            }
            yield return new WaitForSeconds(updateDelay);
        }
    }

    private Vector3 CurrentTarget()
    {
        var targets = vision.targets;
        if (targets.Count == 0) return this.transform.position;
        float minDistance = 10000000f;
        var target = targets[0];
        foreach(var item in targets)
        {
            float distance = (item.transform.position - this.transform.position).magnitude;
            if (distance < minDistance)
            {
                target = item;
                minDistance = distance;
            }
        }
        return target.transform.position;
    }

    private void UpdateAnimator()
    {
        Vector2 dir = agent.velocity;
        float angle = Vector2.SignedAngle(dir, Vector2.up);
        float unsigned = Mathf.Abs(angle);
        float smallAngle = upDownAngleThreshold;
        float largeAngle = 180 - upDownAngleThreshold;
        if (dir.magnitude < idleSpeedThreshold)
        {
            string animName = $"Idle{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
            animator.Play(animName);
            return;
        }
        if (unsigned < smallAngle)
        {
            movingDirection = CardinalDirection.Up;
        }
        else if (-largeAngle <= angle && angle <= -smallAngle)
        {
            movingDirection = CardinalDirection.Left;
        }
        else if (smallAngle <= angle && angle <= largeAngle)
        {
            movingDirection = CardinalDirection.Right;
        }
        else if (unsigned > largeAngle)
        {
            movingDirection = CardinalDirection.Down;
        }
        string animationName = $"Run{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
        animator.Play(animationName);
    }

    public bool TryDamage(int damage)
    {
        if (dying) return false;
        if(Health > 0)
        {
            Health -= damage;
            beingDamaged = true;
            if(Health <= 0)
            {
                Die();
                return true;
            }
            string animationName = $"Hurt{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
            animator.Play(animationName);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Die()
    {
        dying = true;
        string animationName = $"Explode{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
        animator.Play(animationName);
    }

    public void BeingDamagedDone()
    {
        beingDamaged = false;
    }

    public void DestroyObject()
    {
        GameObject.Destroy(this.gameObject);
    }
}
