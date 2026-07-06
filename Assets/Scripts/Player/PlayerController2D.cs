using UnityEngine;

namespace HandOnTheLine.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NoiseEmitter))]
    public class PlayerController2D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D rb;
        private Vector2 moveInput;

        public bool IsQuiet { get; private set; }
        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(x, y).normalized;

            if (moveInput.sqrMagnitude > 0.01f)
            {
                FacingDirection = moveInput;
            }

            // Quiet Hang: the player is "quiet" whenever they hold no movement input.
            IsQuiet = moveInput.sqrMagnitude < 0.01f;
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }

        /// <summary>Used by HookGrapple2D / SnapDash2D to temporarily hand off movement control.</summary>
        public void SetMovementEnabled(bool enabled)
        {
            this.enabled = enabled;
            if (!enabled)
            {
                moveInput = Vector2.zero;
            }
        }
    }
}
