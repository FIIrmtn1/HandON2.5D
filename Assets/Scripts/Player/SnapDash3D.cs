using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Snap Spring "super jump": hold SPACE to charge (0-3s), release to launch mostly upward
    /// with a slight forward boost in the facing direction — used to reach rods/shelves per the GDD's
    /// jump-arc height traversal.</summary>
    [RequireComponent(typeof(PlayerController3D))]
    [RequireComponent(typeof(Rigidbody))]
    public class SnapDash3D : MonoBehaviour
    {
        [SerializeField] private float maxChargeTime = 3f;
        [SerializeField] private float maxImpulseForce = 65f;
        [SerializeField] private float verticalRatio = 0.75f;
        [SerializeField] private float forwardRatio = 0.35f;
        [SerializeField] private float dashLockoutTime = 0.8f;

        private PlayerController3D player;
        private Rigidbody rb;
        private float chargeTime;
        private bool charging;
        private float lockoutTimer;

        public float ChargeRatio => maxChargeTime > 0f ? Mathf.Clamp01(chargeTime / maxChargeTime) : 0f;

        private void Awake()
        {
            player = GetComponent<PlayerController3D>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (lockoutTimer > 0f)
            {
                lockoutTimer -= Time.deltaTime;
                if (lockoutTimer <= 0f)
                {
                    player.SetMovementEnabled(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                charging = true;
                chargeTime = 0f;
            }

            if (charging && Input.GetKey(KeyCode.Space))
            {
                chargeTime = Mathf.Min(chargeTime + Time.deltaTime, maxChargeTime);
            }

            if (charging && Input.GetKeyUp(KeyCode.Space))
            {
                Release();
            }
        }

        private void Release()
        {
            charging = false;
            float force = Mathf.Lerp(0f, maxImpulseForce, ChargeRatio);
            Vector3 launchDirection = (Vector3.up * verticalRatio + player.FacingDirection * forwardRatio).normalized;

            player.SetMovementEnabled(false);
            lockoutTimer = dashLockoutTime;
            rb.AddForce(launchDirection * force, ForceMode.Impulse);
            chargeTime = 0f;
        }
    }
}
