using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machia.Player
{
    /* Author: Anthony D'Alesandro
     * 
     * Handles player actions and input.
     */
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        // DASH control variables
        [SerializeField] private float speed = 1500f;
        [SerializeField] private float dashSpeed = 3000f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashDelay = 3f;
        private float lastDashTime;
        private Coroutine dashRoutine;

        private Rigidbody2D rb;
        private InputAction move;
        private bool inputInitialized = false;


        /* Author: Anthony D'Alesandro
         * 
         * Called when script is added onto game object.
         */
        private void Awake()
        {
            lastDashTime = Time.time - dashDelay;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Called before first Update() frame.
         */
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handle movement of camera and lerped towards player
         */
        void Update()
        {
            if (dashRoutine == null)
            {
                Move();
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Binds InputAction to dash functionality.
         */
        public void InitializeDash(InputAction action)
        {
            Debug.Log("bound dash");
            action.performed += Dash;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Binds InputAction to move functionality.
         */
        public void InitializeMove(InputAction action)
        {
            Debug.Log("bound move: " + action);
            this.move = action;
        }

        /* Author: Anthony D'Alesandro
         * 
         * Handle movement vector from keys. Preformed every frame.
         */
        private void Move()
        {
            Debug.Log(move == null);
            if (move == null) return;

            float dt = Time.deltaTime;
            Vector2 key_vector = move.ReadValue<Vector2>();
            Vector2 v = key_vector * speed * dt * 1000;
            Vector3 velocity = new Vector3(v.x, v.y, 0);
            rb.AddForce(velocity);
        }

        /* Author: Anthony D'Alesandro
         * 
         * Preforms coroutine for dash if allowed.
         */
        private void Dash(InputAction.CallbackContext context)
        {
            if (move == null) return;

            float time = Time.time;
            if (dashRoutine == null && time - lastDashTime >= dashDelay)
            {
                dashRoutine = StartCoroutine(DashRoutine(move.ReadValue<Vector2>()));
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
            if (key_vector.magnitude == 0) yield return null;
            while((transform.position - start).magnitude < dashDistance)
            {
                Vector2 keyForce = key_vector * dashSpeed * Time.deltaTime * 1000;
                Vector3 force = new Vector3(keyForce.x, keyForce.y, 0);
                if(rb) rb.AddForce(force);
                yield return new WaitForEndOfFrame();
            }
            lastDashTime = Time.time;
            dashRoutine = null;
            yield return null;
        }
    }
}