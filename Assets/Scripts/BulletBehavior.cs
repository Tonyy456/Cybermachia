using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float aliveTime = 5f;
    private Rigidbody2D rb;
    private IEnumerator routine;
    public void Initialize(Vector2 movementDirection)
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.AddForce(movementDirection.normalized * speed);
        routine = DestroyInSeconds(aliveTime);
        StartCoroutine(routine);
    }

    public void Explode()
    {
        //play effects?!?
        StopCoroutine(routine);
        GameObject.Destroy(this.gameObject);
    }


    public IEnumerator DestroyInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject.Destroy(this.gameObject);
    }

}
