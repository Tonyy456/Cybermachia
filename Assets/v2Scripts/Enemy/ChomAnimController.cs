using System;
using UnityEngine;
public enum CardinalDirection
{
    Up,
    Down,
    Left,
    Right
}

public class ChomAnimController : MonoBehaviour
{
    [Header("Required Fields")]
    [SerializeField] private bool useLateUpdate = false;
    [SerializeField] private Animator animator;
    [Range(0f,1f)]
    [SerializeField] private float idleSpeedThreshold = 0.05f;
    [Range(0f, 90f)]
    [SerializeField] private float upDownAngleThreshold = 45f;
    [SerializeField] private Rigidbody2D rb;

    private CardinalDirection movingDirection = CardinalDirection.Down;

    private void LateUpdate()
    {
        if(useLateUpdate) UpdateAnimation();
    }
    public void PlayDeath()
    {
        string animName = $"Explode{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
        animator.Play(animName);
    }
    public void UpdateAnimation()
    {
        Vector2 dir = rb.velocity;
        float angle =  Vector2.SignedAngle(dir, Vector2.up);
        float unsigned = Mathf.Abs(angle);
        float smallAngle = upDownAngleThreshold;
        float largeAngle = 180 - upDownAngleThreshold;
        if (dir.magnitude < idleSpeedThreshold)
        {
            string animName = $"Idle{Enum.GetName(typeof(CardinalDirection), movingDirection)}";
            animator.Play(animName);
            return;
        }
        if(unsigned < smallAngle)
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
}
