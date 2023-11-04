using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TIL_BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public void FireBullet(GameObject spawner, Vector2 targetDirection, Vector2 offset)
    {
        if (prefab == null) return;
        GameObject bullet = GameObject.Instantiate(prefab);
        bullet.transform.position = spawner.transform.position + new Vector3(offset.x, offset.y, 0); //modify later?
        bullet.GetComponent<BulletBehavior>().Initialize(targetDirection, this.gameObject);
        bullet.transform.SetParent(this.transform);
    }

    public void BulletCollides(TIL_Bullet bullet, GameObject recipient)
    {
        // var damage = bullet.damage;
        // recipient.damage(damage)
    }

}
