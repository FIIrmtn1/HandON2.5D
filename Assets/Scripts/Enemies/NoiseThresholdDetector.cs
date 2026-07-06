using UnityEngine;
using HandOnTheLine.Player;

namespace HandOnTheLine.Enemies
{
    /// <summary>Pudding's detector: wakes when the player's noise radius exceeds a threshold
    /// within range, regardless of line of sight.</summary>
    public class NoiseThresholdDetector : MonoBehaviour, IEnemyDetector
    {
        [SerializeField] private float wakeThreshold = 1.5f;
        [SerializeField] private float detectionRange = 3f;

        private NoiseEmitter playerNoise;
        private Transform playerTransform;

        public float SuspicionLevel { get; private set; }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerNoise = player.GetComponent<NoiseEmitter>();
                playerTransform = player.transform;
            }
        }

        private void Update()
        {
            if (playerNoise == null)
            {
                SuspicionLevel = 0f;
                return;
            }

            float distance = Vector2.Distance(transform.position, playerTransform.position);
            bool inRange = distance <= detectionRange;
            bool loud = playerNoise.NoiseRadius >= wakeThreshold;

            SuspicionLevel = (inRange && loud) ? 1f : 0f;
        }
    }
}
