using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPot : MonoBehaviour, ICollectable
{
    [SerializeField] private int healAmount;
    public void Collect(GameObject player)
    {
        HordePlayer script = player.GetComponent<HordePlayer>();
        script.Heal(healAmount);
        Destroy(this.gameObject);
    }
}
