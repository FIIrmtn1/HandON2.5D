using UnityEngine;

namespace HandOnTheLine.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NoiseEmitter : MonoBehaviour
    {
        [SerializeField] private float minNoiseRadius = 0f;
        [SerializeField] private float maxNoiseRadius = 4f;
        [SerializeField] private float maxSpeed = 5f;

        private Rigidbody2D rb;
        private bool quietOverride;

        public float NoiseRadius { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetQuietOverride(bool quiet)
        {
            quietOverride = quiet;
        }

        private void Update()
        {
            if (quietOverride)
            {
                NoiseRadius = 0f;
                return;
            }

            float speed = rb.linearVelocity.magnitude;
            float t = maxSpeed > 0f ? Mathf.Clamp01(speed / maxSpeed) : 0f;
            NoiseRadius = Mathf.Lerp(minNoiseRadius, maxNoiseRadius, t);
        }
    }
}
