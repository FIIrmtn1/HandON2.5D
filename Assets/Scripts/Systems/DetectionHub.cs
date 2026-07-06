using UnityEngine;
using HandOnTheLine.Enemies;

namespace HandOnTheLine.Systems
{
    /// <summary>Aggregates every IEnemyDetector in the scene each frame and reports the
    /// strongest suspicion level to Momo. Keeps enemy scripts decoupled from Momo.</summary>
    public class DetectionHub : MonoBehaviour
    {
        [SerializeField] private MomoController momo;

        private MonoBehaviour[] detectorBehaviours;

        private void Start()
        {
            var found = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            System.Collections.Generic.List<MonoBehaviour> list = new System.Collections.Generic.List<MonoBehaviour>();
            foreach (var m in found)
            {
                if (m is IEnemyDetector)
                {
                    list.Add(m);
                }
            }
            detectorBehaviours = list.ToArray();
        }

        private void Update()
        {
            if (momo == null || detectorBehaviours == null)
            {
                return;
            }

            float max = 0f;
            foreach (var behaviour in detectorBehaviours)
            {
                if (behaviour is IEnemyDetector detector)
                {
                    max = Mathf.Max(max, detector.SuspicionLevel);
                }
            }

            momo.ReportSuspicion(max);
        }
    }
}
