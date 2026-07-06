using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Drives the "IsWalking" Animator bool from PlayerController3D.IsQuiet, so the
    /// sprite flipbook only plays while the player is actually moving. Sits on the same GameObject
    /// as the Animator; PlayerController3D is looked up on a parent since the visual (sprite +
    /// Animator) lives on a child "Visual" transform separate from the physics root.</summary>
    [RequireComponent(typeof(Animator))]
    public class SpriteWalkAnimator : MonoBehaviour
    {
        private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

        private Animator animator;
        private PlayerController3D player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            player = GetComponentInParent<PlayerController3D>();
        }

        private void Update()
        {
            if (player == null)
            {
                return;
            }
            animator.SetBool(IsWalkingHash, !player.IsQuiet);
        }
    }
}
