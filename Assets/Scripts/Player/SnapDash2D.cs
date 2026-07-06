using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Hold SPACE to charge (0-5s), release to launch an impulse in the facing direction.</summary>
    [RequireComponent(typeof(PlayerController2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class SnapDash2D : MonoBehaviour
    {
        [SerializeField] private float maxChargeTime = 5f;
        [SerializeField] private float maxImpulseForce = 12f;

        private PlayerController2D player;
        private Rigidbody2D rb;
        private float chargeTime;
        private bool charging;

        public float ChargeRatio => maxChargeTime > 0f ? Mathf.Clamp01(chargeTime / maxChargeTime) : 0f;

        private void Awake()
        {
            player = GetComponent<PlayerController2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
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
            rb.AddForce(player.FacingDirection * force, ForceMode2D.Impulse);
            chargeTime = 0f;
        }
    }
}
