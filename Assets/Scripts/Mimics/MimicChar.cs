using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicChar : MonoBehaviour
    {
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform headTarget;

        private Transform rightArmTargetReference;
        private Transform leftArmTargetReference;
        private Transform headTargetReference;
        private Transform bodyReference;

        private Vector3 _initialHeadPosition;

        private void Awake()
        {
            _initialHeadPosition = headTarget.position;
        }

        private void OnEnable()
        {
            MainEventsManager.rightHandTransformUpdate += ReceiveRightArmTarget;
            MainEventsManager.leftHandTransformUpdate += ReceiveLeftArmTarget;
            MainEventsManager.headTransformUpdate += ReceiveHeadTarget;
            MainEventsManager.bodyTransformUpdate += ReceiveBodyReference;
        }
        private void OnDisable()
        {
            MainEventsManager.rightHandTransformUpdate -= ReceiveRightArmTarget;
            MainEventsManager.leftHandTransformUpdate -= ReceiveLeftArmTarget;
            MainEventsManager.headTransformUpdate -= ReceiveHeadTarget;
            MainEventsManager.bodyTransformUpdate -= ReceiveBodyReference;
        }

        private void ReceiveRightArmTarget(Transform _rightArmTarget)
        {
            rightArmTargetReference = _rightArmTarget;
        }
        private void ReceiveLeftArmTarget(Transform _leftArmTarget)
        {
            leftArmTargetReference = _leftArmTarget;
        }
        private void ReceiveHeadTarget(Transform _headTarget)
        {
            headTargetReference = _headTarget;
        }
        private void ReceiveBodyReference(Transform _bodyReference)
        {
            bodyReference = _bodyReference;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 headRightDelta = rightArmTargetReference.position - headTargetReference.position;
            Vector3 headLeftDelta = leftArmTargetReference.position - headTargetReference.position;

            rightArmTarget.position = headTarget.position + headRightDelta;
            leftArmTarget.position = headTarget.position + headLeftDelta;

            rightArmTarget.eulerAngles = rightArmTargetReference.eulerAngles;
            leftArmTarget.eulerAngles = leftArmTargetReference.eulerAngles;
        }
    }
}