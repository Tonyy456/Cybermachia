using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Machia.Player
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [Range(0,1)]
        [SerializeField] private float lerp_speed = 1f;

        //void Start()
        //{

        //}


        void LateUpdate()
        {
            //Vector3 dp = player.position - this.transform.position;
            float cz = this.transform.position.z;
            Vector3 new_position = Vector3.Lerp(this.transform.position, player.position, lerp_speed);
            new_position.z = cz;
            this.transform.position = new_position;
        }
    }
}
