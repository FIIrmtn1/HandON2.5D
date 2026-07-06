using UnityEngine;

namespace HandOnTheLine.Enemies
{
    /// <summary>Chapter 1 tutorial enemy. Sleeps until noise wakes her; cries and moves erratically
    /// with a widened detection radius; falls back asleep ~4s after the last noise event.</summary>
    [RequireComponent(typeof(NoiseThresholdDetector))]
    public class PuddingController : MonoBehaviour
    {
        [SerializeField] private float resleepDelay = 4f;
        [SerializeField] private float awakeMoveSpeed = 1f;
        [SerializeField] private float awakeDetectionMultiplier = 1.5f;

        private NoiseThresholdDetector detector;
        private Rigidbody2D rb;
        private float timeSinceLastNoise;
        private Vector2 wanderDir;

        public bool IsAwake { get; private set; }

        private void Awake()
        {
            detector = GetComponent<NoiseThresholdDetector>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (detector.SuspicionLevel > 0f)
            {
                IsAwake = true;
                timeSinceLastNoise = 0f;
            }
            else if (IsAwake)
            {
                timeSinceLastNoise += Time.deltaTime;
                if (timeSinceLastNoise >= resleepDelay)
                {
                    IsAwake = false;
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }

            if (IsAwake)
            {
                WanderErratically();
            }
        }

        private void WanderErratically()
        {
            if (rb == null)
            {
                return;
            }

            if (Random.value < 0.02f)
            {
                wanderDir = Random.insideUnitCircle.normalized;
            }

            rb.linearVelocity = wanderDir * awakeMoveSpeed;
        }

        /// <summary>Effective detection radius while crying — wider than the base wakeThreshold range.</summary>
        public float GetDetectionMultiplier()
        {
            return IsAwake ? awakeDetectionMultiplier : 1f;
        }
    }
}
