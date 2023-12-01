using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private int numHits = 2;
    [SerializeField] private int points;

    [Header("Movement Controls")]
    [SerializeField] private Vector2 maxDifference;
    [SerializeField] private float currentLerpValue = 0;
    [SerializeField] private int turnsTillDelete = 1;
    [Range(0,1)]
    [SerializeField] private int direction = 1;
    [Range(0.01f,100f)]
    [SerializeField] private float secondsToTurnAround;

    private Vector3 startPosition;
    private Vector3 endPosition;
    public void Init(bool reverseDirection, float speedScale = 1)
    {
        if (reverseDirection) maxDifference *= -1;
        secondsToTurnAround /= speedScale;
        startPosition = this.transform.position;
        endPosition = startPosition + new Vector3(maxDifference.x, maxDifference.y, 0);
        StartCoroutine(BounceRoutine());
        StartCoroutine(DestroyInTurns());
    }
    public IEnumerator DestroyInTurns()
    {
        yield return new WaitForSeconds(secondsToTurnAround * turnsTillDelete);
        GameObject.Destroy(this.gameObject);
    }
    public IEnumerator BounceRoutine()
    {
        while(true)
        {
            currentLerpValue += ((Time.deltaTime * direction)/ secondsToTurnAround);
            if (currentLerpValue > 1) direction = -1;
            if (currentLerpValue < 0) direction = 1;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, currentLerpValue);
            yield return new WaitForEndOfFrame();
        }
    }
    public void GiveScore(PlayerInput input)
    {
        var controller = GameObject.FindObjectOfType<TargetShooterGameController>();
        controller.OnPlayerScore(input, points);
        numHits -= 1;
        if (numHits <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }
}
