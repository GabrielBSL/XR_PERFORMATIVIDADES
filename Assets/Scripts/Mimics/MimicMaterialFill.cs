using Main.Events;
using Main.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

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

            _rawDelta += JangadaMovement.TotalDelta;
            _completion = Mathf.Clamp01((_rawDelta - totalDeltaThreshold) / totalMovementToFill);
            _materialCopy.SetFloat("_completion", _completion);
        }
    }
}
