using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageEntity : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int numberOfHits = 1;
    [SerializeField] private bool hitMultipleOnSingleTarget = false;

    private List<Collider2D> targetsHit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hitMultipleOnSingleTarget && targetsHit.Contains(collision)) return;
        var component = collision.GetComponent<IDamageable>();
        if (component == null) return;

        if(component.TryDamage(damage))
        {
            targetsHit.Add(collision);
        }
        if (targetsHit.Count >= numberOfHits)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
