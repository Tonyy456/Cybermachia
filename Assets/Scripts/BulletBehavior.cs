using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float aliveTime = 5f;
    [SerializeField] private float minTimeAlive = 0.1f;

    private Rigidbody2D rb;
    private IEnumerator routine;
    private GameObject spawnedFrom;
    private float spawnedTime;

    public void Initialize(Vector2 movementDirection, GameObject spawnedFrom)
    {
        spawnedTime = Time.time;
        this.spawnedFrom = spawnedFrom;
        rb = this.GetComponent<Rigidbody2D>();
        rb.AddForce(movementDirection.normalized * speed);
        routine = DestroyInSeconds(aliveTime);
        StartCoroutine(routine);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == spawnedFrom) return;

        // Is a player
        PlayerController state = collision.gameObject.GetComponent<PlayerController>();
        if(state)
        {
            state.DamagePlayer(3);
            Explode();
            return;
        }
       
        // Just a random object.
        if ((Time.time - spawnedTime) > minTimeAlive) 
        {     
            Explode();
        }
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
