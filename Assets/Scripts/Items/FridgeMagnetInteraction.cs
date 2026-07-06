using UnityEngine;
using HandOnTheLine.Enemies;

namespace HandOnTheLine.Items
{
    /// <summary>Tier 1 environmental interaction. Press E near the fridge magnet to
    /// briefly jam Dusty's direction sensor.</summary>
    public class FridgeMagnetInteraction : MonoBehaviour
    {
        [SerializeField] private float interactRadius = 1f;
        [SerializeField] private ProximityDetector targetDetector;

        private Transform playerTransform;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Update()
        {
            if (playerTransform == null || !Input.GetKeyDown(KeyCode.E))
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance <= interactRadius && targetDetector != null)
            {
                targetDetector.Jam();
                Debug.Log("Fridge Magnet used: Dusty's sensor redirected.");
            }
        }
    }
}
