using HandOnTheLine.Systems;
using UnityEditor;
using UnityEngine;

namespace HandOnTheLine.EditorTools
{
    /// <summary>Adds a HookPoint at each of the 4 top corners of every selected object's renderer bounds,
    /// so Bendy can hook onto edges/shelf lips per the GDD without hand-placing every marker.</summary>
    public static class HookPointGenerator
    {
        private const float TopInset = 0.05f;
        private const float MarkerRadius = 0.15f;

        [MenuItem("Tools/HandOnTheLine/Add Hook Points To Selection")]
        private static void AddHookPoints()
        {
            int hookableLayer = LayerMask.NameToLayer("Hookable");

            foreach (GameObject go in Selection.gameObjects)
            {
                Renderer renderer = go.GetComponentInChildren<Renderer>();
                if (renderer == null)
                {
                    continue;
                }

                Bounds bounds = renderer.bounds;
                float topY = bounds.max.y + TopInset;
                Vector3[] corners =
                {
                    new Vector3(bounds.min.x, topY, bounds.min.z),
                    new Vector3(bounds.min.x, topY, bounds.max.z),
                    new Vector3(bounds.max.x, topY, bounds.min.z),
                    new Vector3(bounds.max.x, topY, bounds.max.z),
                };

                GameObject parent = new GameObject("HookPoints");
                Undo.RegisterCreatedObjectUndo(parent, "Add Hook Points");
                parent.transform.SetParent(go.transform, true);

                for (int i = 0; i < corners.Length; i++)
                {
                    GameObject hook = new GameObject($"HookPoint_{i}");
                    Undo.RegisterCreatedObjectUndo(hook, "Add Hook Points");
                    hook.tag = "HookPoint";
                    hook.layer = hookableLayer;
                    hook.transform.SetParent(parent.transform, true);
                    hook.transform.position = corners[i];

                    SphereCollider marker = hook.AddComponent<SphereCollider>();
                    marker.isTrigger = true;
                    marker.radius = MarkerRadius;

                    hook.AddComponent<HookPointMarker>();
                }
            }

            Debug.Log($"Added hook points to {Selection.gameObjects.Length} object(s).");
        }
    }
}
