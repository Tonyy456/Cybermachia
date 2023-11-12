using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class TIL_BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private TIL_GameStatistics stats;
    public void FireBullet(GameObject spawner, Vector2 targetDirection, Vector2 offset)
    {
        if (prefab == null) return;
        GameObject bullet = GameObject.Instantiate(prefab);
        bullet.transform.position = spawner.transform.position + new Vector3(offset.x, offset.y, 0); //modify later?
        bullet.GetComponent<TIL_Bullet>().Initialize(spawner, targetDirection, this);
        bullet.transform.SetParent(this.transform);
    }

    // Nice to track this information and other statistics.
    public void BulletCollides(TIL_Bullet bullet, GameObject recipient)
    {

        TIL_PlayerController controller = recipient.GetComponent<TIL_PlayerController>();
        bool hisOwnBullet = recipient == bullet.SpawnedFrom;
        if (controller != null && !hisOwnBullet && controller.Damageable)
        {
            controller.UpdateHealth(-1 * bullet.damage);
            PlayerInput shooter = bullet.SpawnedFrom.GetComponent<PlayerInput>();
            PlayerInput shootee = recipient.GetComponent<PlayerInput>();
            stats.PlayerHitPlayer(shooter, shootee);
            if(controller.Health <= 0) stats.PlayerKilled(shooter, shootee);
        }
    }

}
