using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Machia.Input;
using UnityEngine.InputSystem;

/* Author: Anthony D'Alesandro
 * 
 * Handles player actions and input.
 */
namespace Machia.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControllerTest : MonoBehaviour, IInputActor
    {
        private PlayerInput input;
        private InputAction dash;
        private InputAction move;
        private Vector2 MoveDir { get => move.ReadValue<Vector2>(); }

        [SerializeField] private float speed = 1500f;
        [SerializeField] private float dashSpeed = 3000f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashDelay = 3f;
        private float lastDashTime;
        private Coroutine dashRoutine;

        private Rigidbody2D rb;

        private void Awake()
        {
            lastDashTime = Time.time;
        }

        public void Initialize(PlayerInput inputManager)
        {
            input = inputManager;
            input.currentActionMap.Enable();

            dash = input.currentActionMap.FindAction("Dash");
            dash.Enable();
            dash.performed += Dash;

            move = input.currentActionMap.FindAction("Move");
            move.Enable();
        }

        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (dashRoutine == null && input)
            {
                Move(MoveDir);
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handle movement vector from keys. Preformed every frame.
         */
        private void Move(Vector2 key_vector)
        {
            float dt = Time.deltaTime;
            Vector2 v = key_vector * speed * dt * 1000;
            Vector3 velocity = new Vector3(v.x, v.y, 0);
            rb.AddForce(velocity);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handle input of dash key bind. 
         */
        private void Dash(InputAction.CallbackContext context)
        {
            float time = Time.time;
            if (dashRoutine == null && time - lastDashTime >= dashDelay)
            {
                dashRoutine = StartCoroutine(DashRoutine(MoveDir));
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Coroutine that preforms dash mechanics. Moves player a total of 
         * dashDistance at a new force of dashSpeed;
         */
        private IEnumerator DashRoutine(Vector2 key_vector)
        {
            Vector3 start = transform.position;
            while((transform.position - start).magnitude < dashDistance)
            {
                Vector2 keyForce = key_vector * dashSpeed * Time.deltaTime * 1000;
                Vector3 force = new Vector3(keyForce.x, keyForce.y, 0);
                rb.AddForce(force);
                yield return new WaitForEndOfFrame();
            }
            lastDashTime = Time.time;
            dashRoutine = null;
            yield return null;
        }
    }
}