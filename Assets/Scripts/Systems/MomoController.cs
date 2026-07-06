using UnityEngine;

namespace HandOnTheLine.Systems
{
    /// <summary>Diegetic UI: Momo's glow colour communicates danger state. No health bar, no meter.</summary>
    public enum MomoGlowState { Gold, GoldDimming, Orange, RedSteady, RedFlashing, White }

    public class MomoController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector2 followOffset = new Vector2(0.4f, 0.4f);
        [SerializeField] private float followLerp = 5f;
        [SerializeField] private float flashSpeed = 6f;

        [Header("Glow Colours")]
        [SerializeField] private Color gold = new Color(1f, 0.85f, 0.3f);
        [SerializeField] private Color goldDimming = new Color(0.8f, 0.65f, 0.25f);
        [SerializeField] private Color orange = new Color(1f, 0.55f, 0.1f);
        [SerializeField] private Color red = new Color(0.9f, 0.15f, 0.1f);
        [SerializeField] private Color white = Color.white;

        private Renderer rend;
        private MomoGlowState state = MomoGlowState.Gold;
        private float whiteTimer;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        public void SetState(MomoGlowState newState, float holdSeconds = 0f)
        {
            state = newState;
            whiteTimer = holdSeconds;
        }

        /// <summary>Called each frame with the strongest nearby enemy's suspicion level (0-1).</summary>
        public void ReportSuspicion(float suspicionLevel)
        {
            if (state == MomoGlowState.White && whiteTimer > 0f)
            {
                return;
            }

            if (suspicionLevel <= 0f)
            {
                state = MomoGlowState.Gold;
            }
            else if (suspicionLevel < 0.5f)
            {
                state = MomoGlowState.Orange;
            }
            else
            {
                state = MomoGlowState.RedSteady;
            }
        }

        private void Update()
        {
            if (followTarget != null)
            {
                Vector3 targetPos = followTarget.position + (Vector3)followOffset;
                transform.position = Vector3.Lerp(transform.position, targetPos, followLerp * Time.deltaTime);
            }

            if (whiteTimer > 0f)
            {
                whiteTimer -= Time.deltaTime;
                if (whiteTimer <= 0f && state == MomoGlowState.White)
                {
                    state = MomoGlowState.Gold;
                }
            }

            if (rend != null)
            {
                rend.material.color = ColourFor(state);
            }
        }

        private Color ColourFor(MomoGlowState s)
        {
            switch (s)
            {
                case MomoGlowState.Gold: return gold;
                case MomoGlowState.GoldDimming: return goldDimming;
                case MomoGlowState.Orange: return orange;
                case MomoGlowState.RedSteady: return red;
                case MomoGlowState.RedFlashing:
                    float pulse = (Mathf.Sin(Time.time * flashSpeed) + 1f) * 0.5f;
                    return Color.Lerp(red, white, pulse);
                case MomoGlowState.White: return white;
                default: return gold;
            }
        }
    }
}
