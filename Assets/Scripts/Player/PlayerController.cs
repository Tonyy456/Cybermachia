using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Machia.Input;

namespace Machia.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private Rigidbody2D rb;
        private MachiaInputActions input;
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            input = new MachiaInputActions();
            input.MinigameInput.Enable();
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Vector2 key_vector = input.MinigameInput.Move.ReadValue<Vector2>();
            Vector2 v = key_vector * speed;
            Vector3 velocity = new Vector3(v.x, v.y, 0);
            //rb.AddForce(velocity); MAYBE? might feel better
            rb.velocity = velocity;
        }
    }
}