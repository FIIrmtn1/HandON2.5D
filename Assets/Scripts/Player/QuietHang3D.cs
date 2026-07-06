using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>3D port of QuietHang2D: exposes whether Bendy is close enough to hanging-object cover
    /// (coat hooks etc.) to be treated as furniture while quiet.</summary>
    [RequireComponent(typeof(PlayerController3D))]
    public class QuietHang3D : MonoBehaviour
    {
        [SerializeField] private float coverCheckRadius = 0.5f;
        [SerializeField] private LayerMask hookableLayer;

        private PlayerController3D player;

        public bool IsHiddenAsFurniture { get; private set; }

        private void Awake()
        {
            player = GetComponent<PlayerController3D>();
        }

        private void Update()
        {
            IsHiddenAsFurniture = player.IsQuiet &&
                Physics.CheckSphere(transform.position, coverCheckRadius, hookableLayer);
        }
    }
}
