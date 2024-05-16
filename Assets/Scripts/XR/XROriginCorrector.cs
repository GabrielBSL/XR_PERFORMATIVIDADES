using Main.Events;
using System.Collections;
using UnityEngine;

namespace Main.XR
{
    public class XROriginCorrector : MonoBehaviour
    {
        [SerializeField] private Transform headSpawn;
        [SerializeField] private Vector3 offset;

        [Header("Floating")]
        [SerializeField] private float floatingDuration = 20;
        [SerializeField] private float floatingHeight = 5;

        [Header("LineRenderers")]
        [SerializeField] private LineRenderer rightHandLineRenderer;
        [SerializeField] private LineRenderer leftHandLineRenderer;

        private Vector3 _initialPosition;

        private bool _allowCorrection = true;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            MainEventsManager.currentHeadPosition += ReceiveCurrentHeadPosition;
            MainEventsManager.currentHeadEulerAngles += ReceiveCurrentHeadEulerAngles;
            MainEventsManager.endOfPath += StartFloating;
        }
        private void OnDisable()
        {
            MainEventsManager.currentHeadPosition -= ReceiveCurrentHeadPosition;
            MainEventsManager.currentHeadEulerAngles -= ReceiveCurrentHeadEulerAngles;
            MainEventsManager.endOfPath -= StartFloating;
        }

        private void ReceiveCurrentHeadPosition(Vector3 headPosition)
        {
            Debug.Log("test 2");
            if (!_allowCorrection)
            {
                Debug.Log("test fail");
                return;
            }

            Debug.Log("test 3");
            Vector3 positionDifference = headSpawn.position - headPosition;
            Debug.Log("head: " + headPosition + " - spawn: " + headSpawn.position + " - difference: " + positionDifference + " xr initial: " + transform.position);
            transform.position = _initialPosition + offset + positionDifference;
            Debug.Log("xr final: " + transform.position);
        }
        private void ReceiveCurrentHeadEulerAngles(Vector3 headEulerAngles)
        {
            if (!_allowCorrection)
            {
                return;
            }

            transform.eulerAngles = new Vector3(0, headEulerAngles.y * -1, 0);
        }
        private void StartFloating()
        {
            if(!_allowCorrection)
            {
                return;
            }

            _allowCorrection = false;
            StartCoroutine(FloatingCoroutine());
        }

        private IEnumerator FloatingCoroutine()
        {
            float t = 0;
            float floatingVelocity = floatingHeight / floatingDuration;

            while(t < floatingDuration)
            {
                yield return null;

                t += Time.deltaTime;
                transform.position += new Vector3(0, floatingVelocity * Time.deltaTime, 0);
            }
        }
    }
}
