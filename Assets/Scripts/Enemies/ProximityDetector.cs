using UnityEngine;
using UnityEngine.SceneManagement;

namespace HandOnTheLine.Enemies
{
    /// <summary>Dusty's detector: a hazard, not an FSM enemy. No Patrol/Suspicious/Chase states.
    /// Contact with the player's collider is an instant chapter reset. Can be temporarily
    /// disabled (Paperclip jam) or permanently avoided (Hook to shelf height).</summary>
    [RequireComponent(typeof(Collider2D))]
    public class ProximityDetector : MonoBehaviour, IEnemyDetector
    {
        [SerializeField] private float jamDuration = 3.5f;

        private float jamTimer;

        public float SuspicionLevel { get; private set; }

        public bool IsJammed => jamTimer > 0f;

        public void Jam()
        {
            jamTimer = jamDuration;
        }

        private void Update()
        {
            if (jamTimer > 0f)
            {
                jamTimer -= Time.deltaTime;
            }

            SuspicionLevel = IsJammed ? 0f : 0f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsJammed || !other.CompareTag("Player"))
            {
                return;
            }

            Debug.Log("Dusty made contact with Bendy. Chapter reset.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
