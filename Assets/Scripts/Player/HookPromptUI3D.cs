using HandOnTheLine.Systems;
using TMPro;
using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Shows a floating "[E]" prompt above the nearest HookPoint when Bendy is within range on
    /// the X/Z plane, ignoring height differences so a hook up on a shelf still prompts from below.</summary>
    public class HookPromptUI3D : MonoBehaviour
    {
        [SerializeField] private float promptRadius = 16f;
        [SerializeField] private float labelHeightOffset = 1.5f;
        [SerializeField] private float fontSize = 34f;
        [SerializeField] private string promptText = "Press E to hook";

        private TextMeshPro promptLabel;

        private void Awake()
        {
            GameObject labelObject = new GameObject("HookPromptLabel");
            promptLabel = labelObject.AddComponent<TextMeshPro>();
            promptLabel.text = promptText;
            promptLabel.fontSize = fontSize;
            promptLabel.alignment = TextAlignmentOptions.Center;
            labelObject.SetActive(false);
        }

        private void Update()
        {
            Transform nearest = FindNearestHookPoint();

            if (nearest != null)
            {
                promptLabel.transform.position = nearest.position + Vector3.up * labelHeightOffset;

                if (Camera.main != null)
                {
                    Vector3 toCamera = promptLabel.transform.position - Camera.main.transform.position;
                    promptLabel.transform.rotation = Quaternion.LookRotation(toCamera);
                }

                promptLabel.gameObject.SetActive(true);
            }
            else
            {
                promptLabel.gameObject.SetActive(false);
            }
        }

        private Transform FindNearestHookPoint()
        {
            Vector2 selfXZ = new Vector2(transform.position.x, transform.position.z);
            Transform nearest = null;
            float nearestSqrDist = promptRadius * promptRadius;

            foreach (HookPointMarker marker in HookPointMarker.Active)
            {
                Vector2 markerXZ = new Vector2(marker.transform.position.x, marker.transform.position.z);
                float sqrDist = (markerXZ - selfXZ).sqrMagnitude;
                if (sqrDist < nearestSqrDist)
                {
                    nearestSqrDist = sqrDist;
                    nearest = marker.transform;
                }
            }

            return nearest;
        }
    }
}
