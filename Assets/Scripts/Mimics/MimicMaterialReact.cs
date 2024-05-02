using Main.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicMaterialReact : MonoBehaviour
    {
        [SerializeField] private Renderer mimicRenderer;
        [SerializeField] private float loopBaseDuration = 12;
        [SerializeField] private float smoothnessLoopDuration = 6;
        [SerializeField, Range(0, 1)] private float smoothnessRange = 1;
        [SerializeField, Range(0, 255)] private float emissionRangeBase = 32;
        [SerializeField, Range(1, 10)] private float loopDurationMultiplier = 3;
        [SerializeField, Range(1, 10)] private float smoothnessDurationMultiplier = 3;
        [SerializeField, Range(1, 10)] private float emissionMultiplier = 5;
        [SerializeField, Range(.1f, 5)] private float movementDeltaDuration = 3;
        [SerializeField] private float maxMultiplierMovementDelta = 30f;

        private Material _materialCopy;
        private List<PoseInfo> _poses = new List<PoseInfo>();
        private float _currentDelta;
        private float _currentDuration;
        private float _finalDeltaLerp;

        // Start is called before the first frame update
        void Start()
        {
            _materialCopy = new Material(mimicRenderer.material);
            mimicRenderer.material = _materialCopy;

            _materialCopy.EnableKeyword("_EMISSION");
            StartCoroutine(materialEmissionCoroutine());
            StartCoroutine(materialSmoothnessCoroutine());
        }

        private void Update()
        {
            CalculateMovementDelta();
        }

        private void CalculateMovementDelta()
        {
            _poses.Add(RewindController.GetLastPose());
            if (_currentDuration >= movementDeltaDuration)
            {
                _poses.RemoveAt(0);
            }
            else
            {
                _currentDuration += Time.deltaTime;
            }

            Vector3 deltaVector = Vector3.zero;

            for (int i = 0; i < _poses.Count - 1; i++)
            {
                deltaVector += new Vector3(Mathf.Abs(_poses[i + 1].rightTargetLocalPos.x - _poses[i].rightTargetLocalPos.x),
                                           Mathf.Abs(_poses[i + 1].rightTargetLocalPos.y - _poses[i].rightTargetLocalPos.y),
                                           Mathf.Abs(_poses[i + 1].rightTargetLocalPos.z - _poses[i].rightTargetLocalPos.z));

                deltaVector += new Vector3(Mathf.Abs(_poses[i + 1].leftTargetLocalPos.x - _poses[i].leftTargetLocalPos.x),
                                           Mathf.Abs(_poses[i + 1].leftTargetLocalPos.y - _poses[i].leftTargetLocalPos.y),
                                           Mathf.Abs(_poses[i + 1].leftTargetLocalPos.z - _poses[i].leftTargetLocalPos.z));
            }

            _currentDelta = Vector3.Distance(Vector3.zero, deltaVector);
            _finalDeltaLerp = _currentDelta / maxMultiplierMovementDelta;
        }

        private IEnumerator materialEmissionCoroutine()
        {
            float colorChangeDuration = loopBaseDuration / 6;

            float[] colorMult = new float[3] {1, 0, 0};
            float[] colorCurrent = new float[3] {0, 0, emissionRangeBase};

            int colorIndex = 0;
            float baseDurationMultiplier = emissionRangeBase / colorChangeDuration;

            while (true)
            {
                float t = 0;

                while (t < colorChangeDuration)
                {
                    float emissionCurrentMultiplier = Mathf.Lerp(1, emissionMultiplier, _finalDeltaLerp);
                    float durationMultiplier = Mathf.Lerp(1, loopDurationMultiplier, _finalDeltaLerp);

                    t = Mathf.Clamp(t + Time.deltaTime * durationMultiplier, 0, colorChangeDuration);

                    yield return null;

                    for(int i = 0; i < 3; i++)
                    {
                        float correctedValue = colorCurrent[i] + Time.deltaTime * colorMult[i] * baseDurationMultiplier * durationMultiplier * emissionCurrentMultiplier;
                        colorCurrent[i] = Mathf.Clamp(correctedValue, 0, emissionRangeBase * emissionCurrentMultiplier);
                    }

                    Color32 newEmissionColor = new Color32((byte)colorCurrent[0], (byte)colorCurrent[1], (byte)colorCurrent[2], 0);

                    _materialCopy.SetColor("_EmissionColor", newEmissionColor);
                }

                if(colorMult.Any(c => c > 0))
                {
                    colorMult[colorIndex] = 0;
                    colorIndex--;

                    if (colorIndex < 0)
                    {
                        colorIndex = 2;
                    }
                    colorMult[colorIndex] = -1;
                }
                else
                {
                    colorMult[colorIndex] = 0;
                    colorIndex = (colorIndex + 2) % 3;

                    colorMult[colorIndex] = 1;
                }

                yield return null;
            }
        }
        private IEnumerator materialSmoothnessCoroutine()
        {
            float halfLoop = smoothnessLoopDuration / 2;
            float i = 0;
            
            while(true)
            {
                yield return null;

                while (i < halfLoop && i >= 0)
                {
                    smoothnessDurationMultiplier = Mathf.Lerp(smoothnessDurationMultiplier, 0, _finalDeltaLerp);

                    i = Mathf.Clamp(i + Time.deltaTime * smoothnessDurationMultiplier, 0, halfLoop);
                    yield return null;

                    _materialCopy.SetFloat("_Glossiness", Mathf.Lerp(0, smoothnessRange, i / halfLoop));
                }
            }

        }
    }
} 