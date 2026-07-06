using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Rotates a flat 2D sprite to face the camera around the world Y axis, so 2D character
    /// art reads correctly while standing in the 3D 2.5D world. Pairs with PlayerController3D, whose
    /// Rigidbody has rotation fully frozen so this component owns visual rotation exclusively.</summary>
    public class SpriteBillboard : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;

        private void LateUpdate()
        {
            Camera cam = targetCamera != null ? targetCamera : Camera.main;
            if (cam == null)
            {
                return;
            }

            Vector3 toCamera = cam.transform.position - transform.position;
            toCamera.y = 0f;

            if (toCamera.sqrMagnitude < 0.0001f)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(-toCamera, Vector3.up);
        }
    }
}
