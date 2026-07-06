using UnityEngine;

namespace HandOnTheLine.Systems
{
    /// <summary>Fires once when Bendy crosses into range. Placeholder for the Ch3 ambient
    /// voice line until real audio is sourced — logs the line and dims Momo.</summary>
    public class AmbientLineTrigger : MonoBehaviour
    {
        [SerializeField] private MomoController momo;
        [SerializeField] private string line = "\"...things that aren't useful anymore... you just throw them away.\"";

        private bool triggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered || !other.CompareTag("Player"))
            {
                return;
            }

            triggered = true;
            Debug.Log($"[Ambient] {line}");

            if (momo != null)
            {
                momo.SetState(MomoGlowState.GoldDimming);
            }
        }
    }
}
