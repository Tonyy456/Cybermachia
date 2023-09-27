using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Machia.Input;
using UnityEngine.InputSystem;

namespace Machia.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float dash_factor = 5f;

        private Rigidbody2D rb;
        private MachiaInputActions input;
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            input = new MachiaInputActions();
            input.MinigameInput.Enable();

            var dash = input.MinigameInput.Dash;
            dash.performed += Dash;
            dash.Enable();
        }

        private void Dash(InputAction.CallbackContext context)
        {
            Vector2 force = input.MinigameInput.Move.ReadValue<Vector2>() * dash_factor;
            rb.AddForce(force);
            Debug.Log(force.magnitude);
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Vector2 key_vector = input.MinigameInput.Move.ReadValue<Vector2>();
            Vector2 v = key_vector * speed;
            Vector3 velocity = new Vector3(v.x, v.y, 0);
            rb.AddForce(velocity); 
            //rb.velocity = velocity;
        }
    }
}