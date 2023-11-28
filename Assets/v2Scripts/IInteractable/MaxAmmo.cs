using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : MonoBehaviour, ICollectable
{
    [SerializeField] private bool DoForAllPlayers = true;
    public void Collect(GameObject player)
    {
        if (DoForAllPlayers) CollectForAll();
        else CollectForOne(player);
        Destroy(this.gameObject);
    }

    private void CollectForAll()
    {
        v2_AttackManager[] managers = GameObject.FindObjectsOfType<v2_AttackManager>(true);
        foreach(var manager in managers)
        {
            manager.ResetAmmo();
        }
    }

    private void CollectForOne(GameObject player)
    {
        var attackController = player.GetComponent<v2_AttackManager>();
        attackController.ResetAmmo();
    }

}
