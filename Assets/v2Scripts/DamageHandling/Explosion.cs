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

    public void Explode()
    {
        
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.forward);
        if(attenuationOnDistance)
        {
            foreach (var hit in hits)
            {
                float distanceToObstacle = hit.distance;
                var component = hit.transform.GetComponent<IDamageable>();
                component?.TryDamage(AttenuatedDamage(distanceToObstacle));
            }
        } else
        {
            foreach (var hit in hits)
            {
                var component = hit.transform.GetComponent<IDamageable>();
                component?.TryDamage(damage);
            }
        }

    }

    public int AttenuatedDamage(float distance)
    {
        float damageResult = (float)damage / distance;
        return (int)damageResult;
    }
}
