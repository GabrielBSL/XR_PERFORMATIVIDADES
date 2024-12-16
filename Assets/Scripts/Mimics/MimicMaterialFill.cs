using Main.Scenario;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicMaterialFill : MonoBehaviour
    {
        [SerializeField] private Renderer mimicRenderer;
        [SerializeField] private float totalDeltaThreshold;
        [SerializeField] private float totalMovementToFill;
        [SerializeField] private bool startMovementCaptureOnStart = true;

        public bool ToUpdate { get; set; }

        private Material _materialCopy;
        private float _completion = 0;
        private float _rawDelta = 0;

        // Start is called before the first frame update
        void Start()
        {
            if (mimicRenderer == null)
            {
                enabled = false;
                return;
            }

            ToUpdate = startMovementCaptureOnStart;

            _materialCopy = new Material(mimicRenderer.material);
            mimicRenderer.material = _materialCopy;

            _materialCopy.EnableKeyword("_EMISSION");
            _materialCopy.SetFloat("_dissolve", 1);
        }

        // Update is called once per frame
        void Update()
        {
            if(!ToUpdate)
            {
                return;
            }

            _rawDelta = Mathf.Min(JangadaMovement.TotalDelta, _rawDelta);
            _completion = Mathf.Clamp01((_rawDelta - totalDeltaThreshold) / totalMovementToFill);
            _materialCopy.SetFloat("_completion", _completion);
        }
    }
}
