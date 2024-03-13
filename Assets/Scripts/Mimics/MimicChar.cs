using Main.Events;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicChar : MonoBehaviour
    {
        private struct HandInfo
        {
            public Vector3 up;
            public Vector3 forward;
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

        [Header("Debug")]
        [SerializeField] private bool showDebugMessages;

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

        private float _lastReferenceHeight;

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
            _lastReferenceHeight = bodyReference.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            if(bodyReference == null || leftArmTarget == null || rightArmTarget == null)
            {
                return;
            }

            variationDurationTimer -= Time.deltaTime;
            if(variationDurationTimer <= 0)
            {
                variationDurationTimer = variationDuration;
                SetRandomPositionVariation();
            }

            StartCoroutine(DelayMovementReading());
            MoveToDestination();

            //Debug.Log(transform.forward);
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
                up = rightArmTargetReference.up,
                forward = rightArmTargetReference.forward,
                localPosition = rightArmTargetReference.localPosition,
                globalPosition = rightArmTargetReference.position,
            };
            HandInfo leftHandInfo = new HandInfo()
            {
                up = leftArmTargetReference.up,
                forward = leftArmTargetReference.forward,
                localPosition = leftArmTargetReference.localPosition,
                globalPosition = leftArmTargetReference.position,
            };

            Vector3 bodyReferencePosition = bodyReference.position;
            Vector3 bodyReferenceForward = bodyReference.forward;

            yield return new WaitForSeconds(movementDelay);

            //body Height
            float referenceBodyHeightDelta = bodyReferencePosition.y - _lastReferenceHeight;
            transform.position += new Vector3(0, referenceBodyHeightDelta, 0);
            _lastReferenceHeight = bodyReferencePosition.y;

            //body Rotation
            Vector3 direction = bodyReferencePosition - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
            newRotation.x = transform.rotation.x;
            newRotation.z = transform.rotation.z; 
            transform.rotation = newRotation;

            SetHandsRotationAndPosition(rightHandInfo, leftHandInfo, bodyReferenceForward);
        }

        private void SetHandsRotationAndPosition(HandInfo rightHandInfo, HandInfo leftHandInfo, Vector3 bodyReferenceForward)
        {
            float bodyReferenceAngle = Vector2.Angle(new Vector2(bodyReferenceForward.x, bodyReferenceForward.z), new Vector2(0, 1));
            float bodyMimicAngle = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(0, 1));

            bodyReferenceAngle = bodyReferenceForward.x > .001f ? 360 - bodyReferenceAngle : bodyReferenceAngle;
            bodyMimicAngle = transform.forward.x > .001f ? 360 - bodyMimicAngle : bodyMimicAngle;

            SetUpHandRotation(rightArmTarget, bodyMimicAngle, bodyReferenceAngle, leftHandInfo.up, leftHandInfo.forward);
            SetUpHandRotation(leftArmTarget, bodyMimicAngle, bodyReferenceAngle, rightHandInfo.up, rightHandInfo.forward);

            Vector3 rhiLp = rightHandInfo.localPosition;
            Vector3 lhiLp = leftHandInfo.localPosition;

            float randomRightDeltaMultiplier = UnityEngine.Random.Range(movementDeltaVariation.x, movementDeltaVariation.y);
            leftHandDestination.localPosition = GetHandPositionDestination(rhiLp, leftHandDestination.localPosition, _lastRightHandLocalPos, randomRightDeltaMultiplier);

            float randomLeftDeltaMultiplier = UnityEngine.Random.Range(movementDeltaVariation.x, movementDeltaVariation.y);
            rightHandDestination.localPosition = GetHandPositionDestination(lhiLp, rightHandDestination.localPosition, _lastLeftHandLocalPos, randomLeftDeltaMultiplier);

            _lastRightHandLocalPos = rhiLp;
            _lastLeftHandLocalPos = lhiLp;
        }

        //Calculates the delta of the reference hand position and returns the expected current position
        private Vector3 GetHandPositionDestination(Vector3 refHandPos, Vector3 handDest, Vector3 lastHandPos, float deltaMultiplier)
        {
            Vector3 handLocalDelta = refHandPos - lastHandPos;
            handLocalDelta = new Vector3(-handLocalDelta.x, 0, handLocalDelta.z) * deltaMultiplier;
            return new Vector3(handDest.x, refHandPos.y, handDest.z) + handLocalDelta;
        }

        //Calculates the hand "transform.up" and "transform.forward" based on the reference hand rotation
        private void SetUpHandRotation(Transform handTransform, float bodyAngle, float bodyReferenceAngle, Vector3 handReferenceUp, Vector3 handReferenceForward)
        {
            Vector3 handTransformUp = FindLocalAngleByDirection(bodyAngle, bodyReferenceAngle, handReferenceUp);
            Vector3 handTransformForward = FindLocalAngleByDirection(bodyAngle, bodyReferenceAngle, handReferenceForward);

            handTransform.rotation = Quaternion.LookRotation(handTransformForward, handTransformUp);
        }

        //Takes the local direction of a transform and converts to a local direction to other transform, takes both the reference transform angle and the transform of who called
        //X value inverted to give the "mirror" effect
        private Vector3 FindLocalAngleByDirection(float bodyAngle, float bodyReferenceAngle, Vector3 handReferenceDirection)
        {
            Vector3 localReferenceTranformDirection = RotateVectorYAxis(handReferenceDirection, (360 - bodyReferenceAngle));
            Vector3 rotatedDirection = RotateVectorYAxis(localReferenceTranformDirection, (360 - bodyAngle));
            return Vector3.Scale(rotatedDirection, new Vector3(-1, 1, 1));
        }
        
        // Rotates a vector by a degree amount in the y axis
        public Vector3 RotateVectorYAxis(Vector3 originalVector, float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float rotatedX = originalVector.x * Mathf.Cos(angleRadians) - originalVector.z * Mathf.Sin(angleRadians);
            float rotatedZ = originalVector.x * Mathf.Sin(angleRadians) + originalVector.z * Mathf.Cos(angleRadians);

            return new Vector3(rotatedX, originalVector.y, rotatedZ);
        }
    }
}