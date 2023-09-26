using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Machia.Input;

namespace Machia.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxspeed = 10f;
        [SerializeField] private float acceleration = 1f;
        [SerializeField] private float fake_drag_coef = 1f;

        /* PHYSIC LAWS
         * F = M * A, but assume M = 1
         * Drag = 1/2 * p * v2 * A * C, use 1/2v * p here for simplification.
         * F_total = F - Drag
         * da = a0
         * dv = v0 + (a * t)
         * dx = x0 + (v0 * t) + (1/2 * a * t^2)
         * 
         * increase mass to accelerate slower? kinda pointless
         */
        private Vector2 velocity = Vector2.zero;
        private MachiaInputActions input;

        void Start()
        {
            input = new MachiaInputActions();
            input.MinigameInput.Enable();
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Vector2 key_vector = input.MinigameInput.Move.ReadValue<Vector2>();
            Vector2 dp = Vector2.zero;
            if (key_vector.magnitude > 0)
            {
                Vector2 a = key_vector * acceleration;
                velocity += Vector2.ClampMagnitude(a * dt, maxspeed);
                dp = velocity * dt + (0.5f * a * dt * dt);
                
            }
            else
            {
                velocity /= fake_drag_coef * dt;
                dp = velocity * dt;
            }

            //dx
            this.transform.position += new Vector3(dp.x, dp.y, 0);
        }
    }
}