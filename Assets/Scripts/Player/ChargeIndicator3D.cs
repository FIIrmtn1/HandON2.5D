using UnityEngine;

namespace HandOnTheLine.Player
{
    /// <summary>Floating world-space bar above Bendy showing SnapDash3D charge progress; pulses toward a
    /// hot color once max charge is reached so players know holding longer won't add more power.</summary>
    [RequireComponent(typeof(SnapDash3D))]
    public class ChargeIndicator3D : MonoBehaviour
    {
        [SerializeField] private float barWidth = 1.5f;
        [SerializeField] private float barHeight = 0.15f;
        [SerializeField] private float heightOffset = 2.2f;
        [SerializeField] private Color normalColor = Color.yellow;
        [SerializeField] private Color maxColor = Color.red;
        [SerializeField] private float maxPulseSpeed = 10f;

        private SnapDash3D snapDash;
        private Transform pivot;
        private Transform fill;
        private MeshRenderer fillRenderer;
        private MaterialPropertyBlock propertyBlock;

        private void Awake()
        {
            snapDash = GetComponent<SnapDash3D>();

            pivot = new GameObject("ChargeBarPivot").transform;

            GameObject background = GameObject.CreatePrimitive(PrimitiveType.Cube);
            background.name = "ChargeBarBackground";
            Destroy(background.GetComponent<Collider>());
            background.transform.SetParent(pivot, false);
            background.transform.localScale = new Vector3(barWidth, barHeight, 0.01f);
            background.GetComponent<MeshRenderer>().material.color = Color.black;

            GameObject fillObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fillObject.name = "ChargeBarFill";
            Destroy(fillObject.GetComponent<Collider>());
            fillObject.transform.SetParent(pivot, false);
            fillObject.transform.localScale = new Vector3(0f, barHeight * 0.8f, 0.02f);
            fill = fillObject.transform;
            fillRenderer = fillObject.GetComponent<MeshRenderer>();

            propertyBlock = new MaterialPropertyBlock();

            pivot.gameObject.SetActive(false);
        }

        private void Update()
        {
            float ratio = snapDash.ChargeRatio;
            bool active = ratio > 0f;
            pivot.gameObject.SetActive(active);

            if (!active)
            {
                return;
            }

            pivot.position = transform.position + Vector3.up * heightOffset;

            if (Camera.main != null)
            {
                Vector3 toCamera = pivot.position - Camera.main.transform.position;
                pivot.rotation = Quaternion.LookRotation(toCamera);
            }

            float fillWidth = barWidth * ratio;
            fill.localScale = new Vector3(fillWidth, barHeight * 0.8f, 0.02f);
            fill.localPosition = new Vector3(-(barWidth - fillWidth) * 0.5f, 0f, 0f);

            bool atMax = ratio >= 1f;
            Color color = atMax
                ? Color.Lerp(normalColor, maxColor, (Mathf.Sin(Time.time * maxPulseSpeed) + 1f) * 0.5f)
                : Color.Lerp(normalColor, maxColor, ratio);

            propertyBlock.SetColor("_BaseColor", color);
            fillRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
