using System.Collections.Generic;
using UnityEngine;

namespace HandOnTheLine.Systems
{
    /// <summary>Self-registers into a shared list so HookPromptUI3D can scan every hook point in the
    /// scene without a physics query, since prompt proximity is checked on the X/Z plane only regardless
    /// of height (a hook up on a shelf still prompts from directly below).</summary>
    public class HookPointMarker : MonoBehaviour
    {
        public static readonly List<HookPointMarker> Active = new List<HookPointMarker>();

        private void OnEnable()
        {
            Active.Add(this);
        }

        private void OnDisable()
        {
            Active.Remove(this);
        }
    }
}
