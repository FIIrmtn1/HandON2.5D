using UnityEngine;

namespace HandOnTheLine.Enemies
{
    /// <summary>Fixed looping patrol path, consistent speed, no variation.</summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class DustyPatrolController : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float waypointTolerance = 0.15f;

        private Rigidbody2D rb;
        private int currentWaypoint;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                return;
            }

            Transform target = waypoints[currentWaypoint];
            Vector2 pos = rb.position;
            Vector2 toTarget = (Vector2)target.position - pos;

            if (toTarget.magnitude <= waypointTolerance)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                return;
            }

            rb.MovePosition(pos + toTarget.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
