using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetShooterBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public Vector2 moveDir { get; private set; }
    public PlayerInput spawnedFrom { get; private set; }
    public void Init(PlayerInput spawnedFrom, Vector2 direction)
    {
        this.moveDir = direction;
        this.spawnedFrom = spawnedFrom;
        StartCoroutine(moveRoutine());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var comp = collision.GetComponent<TargetBehaviour>();
        if (comp != null)
        {
            comp.GiveScore(spawnedFrom);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    public IEnumerator moveRoutine()
    {
        while(true)
        {
            Vector3 difference = moveDir.normalized * Time.deltaTime * moveSpeed;
            this.transform.position += difference;
            yield return new WaitForEndOfFrame();
        }
    }
}
