using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Bridges PlayerController2D.IsQuiet into NoiseEmitter's quiet override, and exposes
    /// whether Bendy is close enough to hanging-object cover (coat hooks etc.) to be treated as furniture.</summary>
    [RequireComponent(typeof(PlayerController2D))]
    [RequireComponent(typeof(NoiseEmitter))]
    public class QuietHang2D : MonoBehaviour
    {
        [SerializeField] private float coverCheckRadius = 0.5f;
        [SerializeField] private LayerMask hookableLayer;

        private PlayerController2D player;
        private NoiseEmitter noiseEmitter;

        public bool IsHiddenAsFurniture { get; private set; }

        private void Awake()
        {
            player = GetComponent<PlayerController2D>();
            noiseEmitter = GetComponent<NoiseEmitter>();
        }

        private void Update()
        {
            noiseEmitter.SetQuietOverride(player.IsQuiet);

            IsHiddenAsFurniture = player.IsQuiet &&
                Physics2D.OverlapCircle(transform.position, coverCheckRadius, hookableLayer) != null;
        }
    }
}
