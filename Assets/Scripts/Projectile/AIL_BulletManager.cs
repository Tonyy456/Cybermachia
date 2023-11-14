using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class AIL_BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private AIL_GameStatistics stats;
    [SerializeField] private int damageOnCompleteMiss = 1;
    public void FireBullet(GameObject spawner, Vector2 targetDirection, Vector2 offset)
    {
        if (prefab == null) return;
        GameObject bullet = GameObject.Instantiate(prefab);
        bullet.transform.position = spawner.transform.position + new Vector3(offset.x, offset.y, 0); //modify later?
        bullet.GetComponent<AIL_Bullet>().Initialize(spawner, targetDirection, this);
        bullet.transform.SetParent(this.transform);
    }

    public void BulletHitsNothing(AIL_Bullet bullet)
    {
        AIL_PlayerController player = bullet.SpawnedFrom.GetComponent<AIL_PlayerController>();
        player.UpdateHealth(-1 * damageOnCompleteMiss);
    }

    // Nice to track this information and other statistics.
    public void BulletCollides(AIL_Bullet bullet, GameObject recipient)
    {
        AIL_PlayerController controller = recipient.GetComponent<AIL_PlayerController>();
        bool hisOwnBullet = recipient == bullet.SpawnedFrom;
        if (controller != null && !hisOwnBullet)
        {
            controller.UpdateHealth(-1 * bullet.damage);
            PlayerInput shooter = bullet.SpawnedFrom.GetComponent<PlayerInput>();
            PlayerInput shootee = recipient.GetComponent<PlayerInput>();
            stats?.PlayerHitPlayer(shooter, shootee);
            //if(controller.Health <= 0) stats?.PlayerKilled(shooter, shootee);
        }
    }

}
