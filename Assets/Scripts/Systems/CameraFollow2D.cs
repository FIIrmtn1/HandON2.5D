using UnityEngine;

namespace HandOnTheLine.Systems
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float lerp = 4f;
        [SerializeField] private float fixedZ = -10f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desired = new Vector3(target.position.x, target.position.y, fixedZ);
            transform.position = Vector3.Lerp(transform.position, desired, lerp * Time.deltaTime);
        }
    }
}
