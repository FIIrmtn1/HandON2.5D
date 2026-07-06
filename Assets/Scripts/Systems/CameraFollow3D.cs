using UnityEngine;

namespace HandOnTheLine.Systems
{
    /// <summary>Mouse-look third-person orbit camera for the 2.5D format: cursor-locked free look around
    /// the target at a fixed distance, with pitch clamped so the view can't flip over the top or bottom.</summary>
    public class CameraFollow3D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float distance = 6f;
        [SerializeField] private float pivotHeight = 1.5f;
        [SerializeField] private float mouseSensitivity = 3f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 60f;

        private float yaw;
        private float pitch = 20f;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSensitivity, minPitch, maxPitch);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + Vector3.up * pivotHeight;
            transform.position = pivot - rotation * Vector3.forward * distance;
            transform.rotation = rotation;
        }
    }
}
