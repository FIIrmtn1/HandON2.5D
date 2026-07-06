using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>3D port of PlayerController2D for the 2.5D format: movement is camera-relative on the
    /// X/Z ground plane, gravity/height (Y) is left to physics. The Rigidbody's own rotation stays frozen;
    /// visualTransform (the child mesh) rotates independently to face the movement direction.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController3D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 720f;
        [SerializeField] private Transform visualTransform;

        [Header("Noise")]
        [SerializeField] private float minNoiseRadius = 0f;
        [SerializeField] private float maxNoiseRadius = 4f;

        [Header("Fall Safety")]
        [SerializeField] private float fallResetY = -30f;

        private Rigidbody rb;
        private Vector3 moveInput;
        private bool quietOverride;
        private Vector3 spawnPosition;

        public bool IsQuiet { get; private set; }
        public Vector3 FacingDirection { get; private set; } = Vector3.back;
        public float NoiseRadius { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            spawnPosition = rb.position;
        }

        private void Update()
        {
            if (rb.position.y < fallResetY)
            {
                rb.position = spawnPosition;
                rb.linearVelocity = Vector3.zero;
                return;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 input = new Vector3(x, 0f, z);

            if (input.sqrMagnitude > 0.01f && Camera.main != null)
            {
                Transform cam = Camera.main.transform;
                Vector3 camForward = cam.forward;
                camForward.y = 0f;
                camForward.Normalize();
                Vector3 camRight = cam.right;
                camRight.y = 0f;
                camRight.Normalize();
                moveInput = (camForward * z + camRight * x).normalized;
            }
            else
            {
                moveInput = Vector3.zero;
            }

            if (moveInput.sqrMagnitude > 0.01f)
            {
                FacingDirection = moveInput;

                if (visualTransform != null)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
                    visualTransform.rotation = Quaternion.RotateTowards(visualTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
            }

            // Quiet Hang: the player is "quiet" whenever they hold no movement input.
            IsQuiet = quietOverride || moveInput.sqrMagnitude < 0.01f;

            // Noise scales with movement speed: faster movement = louder footsteps.
            float t = IsQuiet ? 0f : moveInput.magnitude;
            NoiseRadius = Mathf.Lerp(minNoiseRadius, maxNoiseRadius, t);
        }

        private void FixedUpdate()
        {
            Vector3 horizontal = moveInput * moveSpeed;
            rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);
        }

        /// <summary>Used by QuietHang3D to force silence even if the player is being moved externally.</summary>
        public void SetQuietOverride(bool quiet)
        {
            quietOverride = quiet;
        }

        /// <summary>Used by HookGrapple3D / SnapDash3D to temporarily hand off movement control.</summary>
        public void SetMovementEnabled(bool enabled)
        {
            this.enabled = enabled;
            if (!enabled)
            {
                moveInput = Vector3.zero;
            }
        }
    }
}
