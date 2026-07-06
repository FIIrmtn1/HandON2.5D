using UnityEngine;
using HandOnTheLine.Systems;

namespace HandOnTheLine.Systems
{
    /// <summary>Placed at a chapter's exit zone (e.g. the Windowsill). Fires the story beat
    /// (Momo glows white) when Bendy reaches it.</summary>
    public class ChapterExitTrigger : MonoBehaviour
    {
        [SerializeField] private MomoController momo;
        [SerializeField] private float whiteGlowHold = 2f;
        [SerializeField] private string chapterCompleteMessage = "Chapter 1 complete: Bendy reaches the Windowsill.";

        private bool triggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered || !other.CompareTag("Player"))
            {
                return;
            }

            triggered = true;
            if (momo != null)
            {
                momo.SetState(MomoGlowState.White, whiteGlowHold);
            }

            Debug.Log(chapterCompleteMessage);
        }
    }
}
