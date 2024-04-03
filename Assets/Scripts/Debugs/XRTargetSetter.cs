using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Main.Debugs
{
    public class XRTargetSetter : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private Transform headController;
        [SerializeField] private Transform leftHandController;
        [SerializeField] private Transform rightHandController;

        [Header("Configurations")]
        [SerializeField] private float headTargetHeight;
        [SerializeField] private float handTargetsHeight;
        [SerializeField] private float rightHandXDistance;
        [SerializeField] private float leftHandXDistance;
        [SerializeField] private float handTargetZAxis;
        [SerializeField] private Vector2 XZOffset;

        private bool updateManually = true;

        private void OnEnable()
        {
            XRDevice.deviceLoaded += OnDeviceLoaded;
        }

        private void OnDisable()
        {
            XRDevice.deviceLoaded -= OnDeviceLoaded;
        }

        private void Update()
        {
            if (!updateManually)
            {
                return;
            }

            headController.transform.localPosition = new Vector3(headController.localPosition.x + XZOffset.x, headTargetHeight, headController.localPosition.z + XZOffset.y);

            leftHandController.transform.localPosition = new Vector3(leftHandXDistance * -1 + XZOffset.x, handTargetsHeight, handTargetZAxis + XZOffset.y);
            rightHandController.transform.localPosition = new Vector3(rightHandXDistance + XZOffset.x, handTargetsHeight, handTargetZAxis + XZOffset.y);
        }

        private void OnDeviceLoaded(string deviceName)
        {
            updateManually = !XRSettings.isDeviceActive;
        }
    }
}
 