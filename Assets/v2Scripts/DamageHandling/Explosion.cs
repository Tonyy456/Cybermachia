using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float radius;
    [SerializeField] public UnityEvent onExplode;
    [SerializeField] private bool attenuationOnDistance = false;
    [SerializeField] private string ignoreTag;

    public void Explode()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, transform.forward, 0f);
        foreach (var hit in hits)
        {
            if (hit.transform.tag == ignoreTag) continue;
            var component = hit.transform.GetComponent<IDamageable>();
            var actualDamage = attenuationOnDistance ? AttenuatedDamage(hit.distance) : damage;
            if (component != null)
            {
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.red, 1f);
                component.TryDamage(actualDamage);
            }
        }

    }

    public int AttenuatedDamage(float distance)
    {
        float damageResult = (float)damage / distance;
        return (int)damageResult;
    }
}
