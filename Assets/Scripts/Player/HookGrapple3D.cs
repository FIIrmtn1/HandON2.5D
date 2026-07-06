using System.Collections;
using HandOnTheLine.Systems;
using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>3D port of HookGrapple2D. Press E near a HookPoint to auto-jump onto it with a short arc;
    /// press E again to release. Proximity is checked on the X/Z plane only (like HookPromptUI3D) since
    /// hook points sit at furniture height, far above where Bendy walks — the jump arc is what bridges
    /// that height gap, so the trigger itself can't require full 3D closeness. While hooked, Bendy is
    /// fully frozen in place at the hook's position — no movement of any kind until released.</summary>
    [RequireComponent(typeof(PlayerController3D))]
    [RequireComponent(typeof(Rigidbody))]
    public class HookGrapple3D : MonoBehaviour
    {
        [SerializeField] private float grabRadius = 3f;
        [SerializeField] private float jumpDuration = 0.4f;
        [SerializeField] private float jumpArcHeight = 2f;
        [SerializeField] private float hangDropHeight = 1.6f;
        [SerializeField] private float hangOutwardOffset = 0.3f;

        private PlayerController3D player;
        private Rigidbody rb;
        private Transform currentHook;
        private bool isJumping;

        public bool IsHooked => currentHook != null;

        private void Awake()
        {
            player = GetComponent<PlayerController3D>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || isJumping)
            {
                return;
            }

            if (IsHooked)
            {
                Release();
                return;
            }

            Transform nearest = FindNearestHookPoint();
            if (nearest != null)
            {
                StartCoroutine(JumpToHook(nearest));
            }
        }

        private Transform FindNearestHookPoint()
        {
            Vector2 selfXZ = new Vector2(transform.position.x, transform.position.z);
            Transform nearest = null;
            float nearestSqrDist = grabRadius * grabRadius;

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

        private IEnumerator JumpToHook(Transform hook)
        {
            isJumping = true;
            player.SetMovementEnabled(false);
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            Vector3 start = rb.position;
            Vector3 outward = start - hook.position;
            outward.y = 0f;
            outward = outward.sqrMagnitude > 0.01f ? outward.normalized : Vector3.zero;

            Vector3 end = hook.position + outward * hangOutwardOffset + Vector3.down * hangDropHeight;
            float elapsed = 0f;

            while (elapsed < jumpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / jumpDuration);
                Vector3 pos = Vector3.Lerp(start, end, t);
                pos.y += jumpArcHeight * 4f * t * (1f - t);
                rb.MovePosition(pos);
                yield return null;
            }

            rb.position = end;
            currentHook = hook;
            isJumping = false;
        }

        private void Release()
        {
            currentHook = null;
            rb.isKinematic = false;
            player.SetMovementEnabled(true);
        }
    }
}
