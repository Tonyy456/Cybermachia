using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIL_Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    [SerializeField] private float aliveTime = 5f;
    [SerializeField] private float minTimeAlive = 0.1f;

    public GameObject SpawnedFrom { get; set; }

    private TIL_BulletManager manager;
    private Rigidbody2D rb;
    private IEnumerator routine;
    private float spawnedTime;

    public void Initialize(GameObject spawnedFrom, Vector2 movementDirection, TIL_BulletManager manager)
    {
        this.manager = manager;
        this.SpawnedFrom = spawnedFrom;
        this.spawnedTime = Time.time;
        this.rb = this.GetComponent<Rigidbody2D>();

        rb.AddForce(movementDirection.normalized * speed);
        routine = DestroyInSeconds(aliveTime);
        StartCoroutine(routine);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == SpawnedFrom) return;
        if (collision.tag == "Player")
        {
            manager.BulletCollides(this, collision.gameObject);
            return;
        }
        if ((Time.time - spawnedTime) > minTimeAlive)
        {
            Explode();
            return;
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
