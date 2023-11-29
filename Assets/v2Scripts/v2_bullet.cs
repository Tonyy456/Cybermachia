using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class v2_bullet : MonoBehaviour
{
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private string ignoreTag = "Player";
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    [SerializeField] private float aliveTime = 5f;
    [SerializeField] private float minTimeAlive = 0.1f;
    [SerializeField] private int numHitsPossible = 2;
    public GameObject SpawnedFrom { get; set; }

    private Rigidbody2D rb;
    private IEnumerator routine;
    private float spawnedTime;
    private List<Collider2D> hitTargets = new List<Collider2D>();
    public void Initialize(GameObject spawnedFrom, Vector2 movementDirection)
    {
        GameObject[] ignoreCollisionsFrom = GameObject.FindGameObjectsWithTag("PlayerOnlyCollider");
        foreach(var item in ignoreCollisionsFrom)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), item.GetComponent<Collider2D>());
        }
        
        this.SpawnedFrom = spawnedFrom;
        this.spawnedTime = Time.time;
        this.rb = this.GetComponent<Rigidbody2D>();

        rb.AddForce(movementDirection.normalized * speed);
        routine = DestroyInSeconds(aliveTime);
        StartCoroutine(routine);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (hitTargets.Contains(collision)) return;
        if (collision.tag == ignoreTag) return;
        if (collision.isTrigger) return;
        if (collision.gameObject == SpawnedFrom) return;
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && collision.tag == targetTag)
        {
            hitTargets.Add(collision);
            damageable.TryDamage(damage);
            if (hitTargets.Count >= numHitsPossible)
            {
                Explode();
            }
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
        if(routine != null) StopCoroutine(routine);
        GameObject.Destroy(this.gameObject);
    }

    public IEnumerator DestroyInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject.Destroy(this.gameObject);
    }
}
