using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Debugs
{
    public class XRTargetSetter : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private Transform headTarget;
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandTarget;

        [Header("Configurations")]
        [SerializeField] private float idealHeight = 1.72f;
        [SerializeField] private float headTargetHeight;
        [SerializeField] private float handTargetsHeight;
        [SerializeField] private float rightHandXDistance;
        [SerializeField] private float leftHandXDistance;
        [SerializeField] private float handTargetZAxis;

        private void Start()
        {
            headTarget.transform.localPosition = new Vector3(headTarget.localPosition.x, headTargetHeight, headTarget.localPosition.z);

            leftHandTarget.transform.localPosition = new Vector3(leftHandXDistance * -1, handTargetsHeight, handTargetZAxis);
            rightHandTarget.transform.localPosition = new Vector3(rightHandXDistance, handTargetsHeight, handTargetZAxis);
        }

        private void OnEnable()
        {
            MainEventsManager.defaultHeightValue += ReceiveInitialHeight;
        }
        private void OnDisable()
        {
            MainEventsManager.defaultHeightValue -= ReceiveInitialHeight;
        }

        private void Update()
        {
            headTarget.transform.localPosition = new Vector3(headTarget.localPosition.x, headTargetHeight, headTarget.localPosition.z);

            leftHandTarget.transform.localPosition = new Vector3(leftHandXDistance * -1, handTargetsHeight, handTargetZAxis);
            rightHandTarget.transform.localPosition = new Vector3(rightHandXDistance, handTargetsHeight, handTargetZAxis);
        }

        private void ReceiveInitialHeight(float height)
        {
            float heightDifference = idealHeight - height;
            transform.position = new Vector3(transform.position.x, transform.position.y + heightDifference, transform.position.z);
        }
    }
}
