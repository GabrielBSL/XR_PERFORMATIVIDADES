using Main.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Mimics
{
    public class MimicMaterialReact : MonoBehaviour
    {
        [SerializeField] private Renderer mimicRenderer;
        [SerializeField] private Color emissionColor;
        [SerializeField] private float loopBaseDuration = 12;
        [SerializeField] private float smoothnessLoopDuration = 6;
        [SerializeField, Range(1, 10)] private float deltaRandomness = 3;
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
        private float _baseRandomness;

        // Start is called before the first frame update
        void Start()
        {
            if(mimicRenderer == null)
            {
                enabled = false;
                return;
            }

            _baseRandomness = Random.Range(1, deltaRandomness);

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
            float currentRandom = Random.Range(1, _baseRandomness);

            _finalDeltaLerp = _currentDelta * currentRandom / maxMultiplierMovementDelta;
        }

        private IEnumerator materialEmissionCoroutine()
        {
            float colorChangeDuration = loopBaseDuration / 2;

            float[] colorCurrent = new float[3];

            int signMultiplier = 1;
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
                    Vector3 normalizedEmission = new Vector3(emissionColor.r, emissionColor.g, emissionColor.b).normalized;
                    float currentMultiplier = Time.deltaTime * signMultiplier * baseDurationMultiplier * durationMultiplier * emissionCurrentMultiplier;

                    for (int i = 0; i < 3; i++)
                    {
                        float correctedValue = colorCurrent[i] + normalizedEmission[i] * currentMultiplier;
                        colorCurrent[i] = Mathf.Clamp(correctedValue, 0, emissionRangeBase * emissionCurrentMultiplier);
                    }

                    Color32 newEmissionColor = new Color32((byte)colorCurrent[0], (byte)colorCurrent[1], (byte)colorCurrent[2], 0);

                    _materialCopy.SetColor("_EmissionColor", newEmissionColor);
                }

                signMultiplier *= -1;
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