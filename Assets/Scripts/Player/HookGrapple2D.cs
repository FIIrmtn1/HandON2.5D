using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Contextual hook-and-swing traversal. Press E near a HookPoint to attach;
    /// press E again (or move away) to release.</summary>
    [RequireComponent(typeof(PlayerController2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class HookGrapple2D : MonoBehaviour
    {
        [SerializeField] private float grabRadius = 1.2f;
        [SerializeField] private float swingSpeed = 4f;
        [SerializeField] private LayerMask hookPointLayer;

        private PlayerController2D player;
        private Rigidbody2D rb;
        private Transform currentHook;

        public bool IsHooked => currentHook != null;

        private void Awake()
        {
            player = GetComponent<PlayerController2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }

            if (IsHooked)
            {
                Release();
                return;
            }

            Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRadius, hookPointLayer);
            if (hit != null && hit.CompareTag("HookPoint"))
            {
                Attach(hit.transform);
            }
        }

        private void Attach(Transform hook)
        {
            currentHook = hook;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            player.SetMovementEnabled(false);
        }

        private void Release()
        {
            currentHook = null;
            rb.gravityScale = 1f;
            player.SetMovementEnabled(true);
        }

        private void FixedUpdate()
        {
            if (!IsHooked)
            {
                return;
            }

            float x = Input.GetAxisRaw("Horizontal");
            Vector2 pos = rb.position;
            pos.x += x * swingSpeed * Time.fixedDeltaTime;
            rb.MovePosition(pos);
        }
    }
}
