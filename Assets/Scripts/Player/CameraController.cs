using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author: Anthony D'Alesandro
 * 
 * Handle movement of camera and lerped towards player
 */
namespace Machia.Player
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Range(0,1)]
        [SerializeField] private float lerp_speed = 1f;
        [SerializeField] private Transform player;

        /* Author: Anthony D'Alesandro
         * 
         * Handle movement of camera every fixed amount of frames.
         */
        void FixedUpdate()
        {
            //Vector3 dp = player.position - this.transform.position;
            float cz = this.transform.position.z;
            Vector3 new_position = Vector3.Lerp(this.transform.position, player.position, lerp_speed);
            new_position.z = cz;
            this.transform.position = new_position;
        }
    }
}
