using Main.Events;
using System;
using System.Collections;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicChar : MonoBehaviour
    {
        private struct HandInfo
        {
            public Vector3 localEulerAngles;
            public Vector3 localPosition;
            public Vector3 globalPosition;
        }

        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;

        [Header("Movement")]
        [SerializeField] private Transform rightHandDestination;
        [SerializeField] private Transform leftHandDestination;
        [SerializeField, Range(.01f, 1f)] private float handMovementSmoothness = .033f;
        [SerializeField] private Vector2 movementDeltaVariation = Vector2.one;
        [SerializeField] private float movementDelay = .5f;

        [Header("Random")]
        [SerializeField] private float positionVariation = .05f;
        [SerializeField] private float variationDuration = .75f;
        [SerializeField, Range(.01f, 1f)] private float variationSmoothness = .033f;

        private Transform rightArmTargetReference;
        private Transform leftArmTargetReference;
        private Transform bodyReference;

        private float variationDurationTimer = 0;

        private Vector3 _lastRightHandLocalPos;
        private Vector3 _lastLeftHandLocalPos;

        private Vector3 _leftHandFinalPositionVariation;
        private Vector3 _rightHandFinalPositionVariation;
        private Vector3 _leftHandCurrentPositionVariation;
        private Vector3 _rightHandCurrentPositionVariation;

        private void OnEnable()
        {
            MainEventsManager.rightHandTransformUpdate += ReceiveRightArmTarget;
            MainEventsManager.leftHandTransformUpdate += ReceiveLeftArmTarget;
            MainEventsManager.bodyTransformUpdate += ReceiveBodyReference;
        }
        private void OnDisable()
        {
            MainEventsManager.rightHandTransformUpdate -= ReceiveRightArmTarget;
            MainEventsManager.leftHandTransformUpdate -= ReceiveLeftArmTarget;
            MainEventsManager.bodyTransformUpdate -= ReceiveBodyReference;
        }
        private void ReceiveRightArmTarget(Transform _rightArmTarget)
        {
            rightArmTargetReference = _rightArmTarget;
            _lastRightHandLocalPos = _rightArmTarget.localPosition;
            rightArmTarget.localPosition = _rightArmTarget.localPosition;
            rightHandDestination.localPosition = _rightArmTarget.localPosition;
        }
        private void ReceiveLeftArmTarget(Transform _leftArmTarget)
        {
            leftArmTargetReference = _leftArmTarget;
            _lastLeftHandLocalPos = _leftArmTarget.localPosition;
            leftArmTarget.localPosition = _leftArmTarget.localPosition;
            leftHandDestination.localPosition = _leftArmTarget.localPosition;
        }
        private void ReceiveBodyReference(Transform _bodyReference)
        {
            bodyReference = _bodyReference;
        }

        // Update is called once per frame
        void Update()
        {
            variationDurationTimer -= Time.deltaTime;
            if(variationDurationTimer <= 0)
            {
                variationDurationTimer = variationDuration;
                SetRandomPositionVariation();
            }

            StartCoroutine(DelayMovementReading());
            MoveToDestination();
        }

        private void SetRandomPositionVariation()
        {
            _rightHandFinalPositionVariation = new Vector3(UnityEngine.Random.Range(-positionVariation, positionVariation),
                                                         UnityEngine.Random.Range(-positionVariation, positionVariation),
                                                         UnityEngine.Random.Range(-positionVariation, positionVariation));

            _leftHandFinalPositionVariation = new Vector3(UnityEngine.Random.Range(-positionVariation, positionVariation),
                                                     UnityEngine.Random.Range(-positionVariation, positionVariation),
                                                     UnityEngine.Random.Range(-positionVariation, positionVariation));
        }

        private void MoveToDestination()
        {
            _rightHandCurrentPositionVariation = Vector3.Lerp(_rightHandCurrentPositionVariation, _rightHandFinalPositionVariation, variationSmoothness);
            _leftHandCurrentPositionVariation = Vector3.Lerp(_leftHandCurrentPositionVariation, _leftHandFinalPositionVariation, variationSmoothness);

            Vector3 rightHandFinalPosition = rightHandDestination.localPosition + _rightHandCurrentPositionVariation;
            Vector3 leftHandFinalPosition = leftHandDestination.localPosition + _leftHandCurrentPositionVariation;

            rightArmTarget.localPosition = Vector3.Lerp(rightArmTarget.localPosition, rightHandFinalPosition, handMovementSmoothness);
            leftArmTarget.localPosition = Vector3.Lerp(leftArmTarget.localPosition, leftHandFinalPosition, handMovementSmoothness);
        }

        private IEnumerator DelayMovementReading()
        {
            HandInfo rightHandInfo = new HandInfo()
            {
                localEulerAngles = rightArmTargetReference.localEulerAngles,
                localPosition = rightArmTargetReference.localPosition,
                globalPosition = rightArmTargetReference.position,
            };
            HandInfo leftHandInfo = new HandInfo()
            {
                localEulerAngles = leftArmTargetReference.localEulerAngles,
                localPosition = leftArmTargetReference.localPosition,
                globalPosition = leftArmTargetReference.position,
            };

            Vector3 bodyReferencePosition = bodyReference.position;

            yield return new WaitForSeconds(movementDelay);

            //body Height
            transform.position = new Vector3(transform.position.x, bodyReferencePosition.y, transform.position.z);

            //body Rotation
            Vector3 direction = bodyReferencePosition - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
            newRotation.x = transform.rotation.x;
            newRotation.z = transform.rotation.z; 
            transform.rotation = newRotation;

            SetHandsRotationAndPosition(rightHandInfo, leftHandInfo);
        }

        private void SetHandsRotationAndPosition(HandInfo rightHandInfo, HandInfo leftHandInfo)
        {
            //Hands angles
            rightArmTarget.localEulerAngles = leftHandInfo.localEulerAngles - new Vector3(0, 0, 180);
            leftArmTarget.localEulerAngles = rightHandInfo.localEulerAngles - new Vector3(0, 0, 180);

            Vector3 rhiLp = rightHandInfo.localPosition;
            Vector3 lhiLp = leftHandInfo.localPosition;
            Vector3 rhdLp = rightHandDestination.localPosition;
            Vector3 lhdLp = leftHandDestination.localPosition;

            Vector3 rightHandLocalDelta = rhiLp - _lastRightHandLocalPos;
            Vector3 leftHandLocalDelta = lhiLp - _lastLeftHandLocalPos;

            float randomRightDeltaMultiplier = UnityEngine.Random.Range(movementDeltaVariation.x, movementDeltaVariation.y);
            float randomLeftDeltaMultiplier = UnityEngine.Random.Range(movementDeltaVariation.x, movementDeltaVariation.y);

            rightHandLocalDelta = new Vector3(-rightHandLocalDelta.x, 0, rightHandLocalDelta.z) * randomRightDeltaMultiplier;
            leftHandLocalDelta = new Vector3(-leftHandLocalDelta.x, 0, leftHandLocalDelta.z) * randomLeftDeltaMultiplier;

            rightHandDestination.localPosition = new Vector3(rhdLp.x, lhiLp.y, rhdLp.z) + leftHandLocalDelta;
            leftHandDestination.localPosition = new Vector3(lhdLp.x, rhiLp.y, lhdLp.z) + rightHandLocalDelta;

            _lastRightHandLocalPos = rhiLp;
            _lastLeftHandLocalPos = lhiLp;
        }
    }
}