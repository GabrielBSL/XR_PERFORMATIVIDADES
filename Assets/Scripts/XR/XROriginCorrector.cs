using Main.Events;
using System.Collections;
using UnityEngine;

namespace Main.XR
{
    public class XROriginCorrector : MonoBehaviour
    {
        [SerializeField] private Transform headSpawn;
        [SerializeField] private Transform cameraTransform;
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
            MainEventsManager.currentPlayerCameraPointPos += UpdateCameraPosition;
            MainEventsManager.endOfPath += StartFloating;
        }
        private void OnDisable()
        {
            MainEventsManager.currentHeadPosition -= ReceiveCurrentHeadPosition;
            MainEventsManager.currentHeadEulerAngles -= ReceiveCurrentHeadEulerAngles;
            MainEventsManager.currentPlayerCameraPointPos -= UpdateCameraPosition;
            MainEventsManager.endOfPath -= StartFloating;
        }

        private void UpdateCameraPosition(Vector3 position)
        {
            cameraTransform.position = position;
        }

        private void ReceiveCurrentHeadPosition(Vector3 headPosition)
        {
            if (!_allowCorrection)
            {
                return;
            }

            Vector3 positionDifference = headSpawn.position - headPosition;
            //transform.position = _initialPosition + offset + positionDifference;
            transform.position += offset + positionDifference;
        }
        private void ReceiveCurrentHeadEulerAngles(Vector3 headEulerAngles)
        {
            if (!_allowCorrection)
            {
                return;
            }

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + headEulerAngles.y * -1, 0);
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
