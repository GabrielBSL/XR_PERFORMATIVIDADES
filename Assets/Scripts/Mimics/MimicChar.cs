using Main.Events;
using Main.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

namespace Main.Mimics
{
    [ExecuteInEditMode]
    public class MimicChar : MonoBehaviour
    {
        private struct HandInfo
        {
            public Vector3 up;
            public Vector3 forward;
            public Vector3 localPosition;
        }

        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform rightArmHint;
        [SerializeField] private Transform leftArmHint;
        [SerializeField] private Transform headTarget;

        [Header("Movement")]
        [SerializeField] private Transform rightHandDestination;
        [SerializeField] private Transform leftHandDestination;
        [SerializeField, Range(.01f, 1f)] private float handMovementSmoothness = .033f;
        [SerializeField] private float movementDelay = .5f;

        [Header("Ground")]
        [SerializeField] private bool automaticallySnapToGround = true;
        [SerializeField, Range(0f, 10f)] private float groundDetectionHeight = 1f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private bool enableBodyPosVariation = true;
        [SerializeField] private Vector2 bodyPosVariation;
        [SerializeField] private Vector2 bodyPosVariationDuration;

        [Header("Walking")]
        [SerializeField] private Transform[] destinations;
        [SerializeField] private float walkVelocity = 3;
        [SerializeField, Range(.01f, 1f)] private float pathSmoothness = .1f;

        [Header("Random")]
        [SerializeField] private float positionVariation = .05f;
        [SerializeField] private float variationDuration = .75f;
        [SerializeField, Range(.01f, 1f)] private float variationSmoothness = .033f;

        [Header("Correction")]
        [SerializeField] private float handsYCorrection;
        [SerializeField] private float handsXCorreciton;
        [SerializeField] private float headYCorrection;

        [Header("Rewind")]
        [SerializeField] private bool autoRewind;
        [SerializeField, Range(0, 5)] private float autoRewindTweenTime = 1f;
        [SerializeField] private bool allowClipRewind;
        [SerializeField, Range(0, 1)] private float rewindChance = .5f;

        [Header("Debug")]
        [SerializeField] private bool showDebugMessages;
        [SerializeField] private bool forceStartPath;

        private Transform _rightArmTargetReference;
        private Transform _leftArmTargetReference;
        private Transform _rightArmHintReference;
        private Transform _leftHandHintReference;
        private Transform _headReference;
        private Transform _bodyReference;

        private float _variationDurationTimer = 0;
        private float _initialReferenceBodyHeight;
        private float _bodyPositionVariationTimer = 0;
        private float _bodyPosCurrentTimerLimit;

        private int _currentPathDestination = 0;

        private Vector3 _destinationPosition;
        private Vector3 _destinationRandomVariation;
        private Vector3 _destinationRandomVariationLerp;
        private Vector3 _leftHandFinalPositionVariation;
        private Vector3 _rightHandFinalPositionVariation;
        private Vector3 _leftHandCurrentPositionVariation;
        private Vector3 _rightHandCurrentPositionVariation;

        private List<PoseInfo> _clipInfo;
        private bool _playingClip;
        private bool _inRewind;
        private bool _followHeadHeight;

        private void Awake()
        {
            _destinationPosition = transform.position;
            _bodyPosCurrentTimerLimit = UnityEngine.Random.Range(bodyPosVariationDuration.x, bodyPosVariationDuration.y);

            _bodyPositionVariationTimer = _bodyPosCurrentTimerLimit;
        }

        private void OnEnable()
        {
            MainEventsManager.rightHandTargetTransformUpdate += ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate += ReceiveLeftArmTarget;
            MainEventsManager.rightHandHintTransformUpdate += ReceiveRightArmHint;
            MainEventsManager.leftHandHintTransformUpdate += ReceiveLeftArmHint;
            MainEventsManager.headTransformUpdate += ReceiveHeadReference;
            MainEventsManager.bodyTransformUpdate += ReceiveBodyReference;
            MainEventsManager.endOfPath += ReceiveEndOfPath;
            RewindController.onStartRewind += ReceiveRewindCommand;
        }
        private void OnDisable()
        {
            MainEventsManager.rightHandTargetTransformUpdate -= ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate -= ReceiveLeftArmTarget;
            MainEventsManager.rightHandHintTransformUpdate -= ReceiveRightArmHint;
            MainEventsManager.leftHandHintTransformUpdate -= ReceiveLeftArmHint;
            MainEventsManager.headTransformUpdate -= ReceiveHeadReference;
            MainEventsManager.bodyTransformUpdate -= ReceiveBodyReference;
            MainEventsManager.endOfPath -= ReceiveEndOfPath;
            RewindController.onStartRewind -= ReceiveRewindCommand;
        }
        private void ReceiveRightArmTarget(Transform _rightArmTarget)
        {
            _rightArmTargetReference = _rightArmTarget;
            rightArmTarget.localPosition = _rightArmTarget.localPosition;
            rightHandDestination.localPosition = _rightArmTarget.localPosition;
        }
        private void ReceiveLeftArmTarget(Transform _leftArmTarget)
        {
            _leftArmTargetReference = _leftArmTarget;
            leftArmTarget.localPosition = _leftArmTarget.localPosition;
            leftHandDestination.localPosition = _leftArmTarget.localPosition;
        }
        private void ReceiveRightArmHint(Transform _rightArmHint)
        {
            _rightArmHintReference = _rightArmHint;
        }
        private void ReceiveLeftArmHint(Transform _leftArmHint)
        {
            _leftHandHintReference = _leftArmHint;
        }
        private void ReceiveBodyReference(Transform _bodyReference)
        {
            this._bodyReference = _bodyReference;
            _initialReferenceBodyHeight = this._bodyReference.position.y;
        }
        private void ReceiveHeadReference(Transform _headReference)
        {
            this._headReference = _headReference;
        }
        private void ReceiveEndOfPath()
        {
            _followHeadHeight = false;
        }

        private void ReceiveRewindCommand()
        {
            float randomChance = UnityEngine.Random.Range(0, 1f);

            if (allowClipRewind && randomChance <= rewindChance)
            {
                StartClipRewind();
            }
        }
        private void StartClipRewind()
        {
            if (!_inRewind)
            {
                float randomChance = UnityEngine.Random.Range(0, 1f);

                if (allowClipRewind && randomChance <= rewindChance)
                {
                    StartClipRewind();
                }
            }
        }

        private void Start()
        {
            float randomChance = UnityEngine.Random.Range(0, 1f);

            if (allowClipRewind && randomChance <= rewindChance)
            {
                StartCoroutine(AutoRewindCoroutine());
            }
        }

        private IEnumerator AutoRewindCoroutine()
        {
            _inRewind = true;

            while (true)
            {
                yield return null;
                if (_playingClip)
                {
                    continue;
                }

                if(_clipInfo == null)
                {
                    _clipInfo = RewindController.GetRandomClip();

                    if (_clipInfo == null)
                    {
                        autoRewind = false;
                        yield break;
                    }

                    continue;
                }

                yield return StartCoroutine(ClipReadingCoroutine());

                float tweenTimer = 0;
                List<PoseInfo> newClip = RewindController.GetRandomClip();

                while (tweenTimer < autoRewindTweenTime)
                {
                    yield return null;

                    tweenTimer += Time.deltaTime;
                    float t = tweenTimer / autoRewindTweenTime;

                    PoseInfo tweenPose = RewindController.PoseLerp(_clipInfo[_clipInfo.Count - 1], newClip[0], t);
                    SetMimicPose(tweenPose);
                }

                _clipInfo = newClip;
                StartCoroutine(ClipReadingCoroutine());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying && automaticallySnapToGround)
            {
                SnapToGround();
            }

            if (_bodyReference == null || leftArmTarget == null || rightArmTarget == null)
            {
                return;
            }

            if (forceStartPath)
            {
                forceStartPath = false;
                GoToNextDestination();
            }

            if(enableBodyPosVariation)
            {
                _bodyPositionVariationTimer += Time.deltaTime;
                if (_bodyPositionVariationTimer > _bodyPosCurrentTimerLimit)
                {
                    _bodyPositionVariationTimer = 0;
                    _bodyPosCurrentTimerLimit = UnityEngine.Random.Range(bodyPosVariationDuration.x, bodyPosVariationDuration.y);

                    float randomX = UnityEngine.Random.Range(-bodyPosVariation.x, bodyPosVariation.x);
                    float randomZ = UnityEngine.Random.Range(-bodyPosVariation.y, bodyPosVariation.y);

                    _destinationRandomVariation = new Vector3(randomX, 0, randomZ);
                }

                _destinationRandomVariationLerp = Vector3.Lerp(_destinationRandomVariationLerp, _destinationRandomVariation, pathSmoothness);
                transform.position = Vector3.Lerp(transform.position, _destinationPosition + _destinationRandomVariationLerp, pathSmoothness);
            }
            
            _variationDurationTimer -= Time.deltaTime;
            if(_variationDurationTimer <= 0)
            {
                _variationDurationTimer = variationDuration;
                SetRandomPositionVariation();
            }

            if(!_playingClip && !_inRewind)
            {
                StartCoroutine(DelayMovementReading());
            }
            MoveToDestination();
        }

        private void SnapToGround(float correction = 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, groundDetectionHeight, 0), Vector3.down, out hit, float.MaxValue, groundLayer))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance + correction + groundDetectionHeight + headYCorrection, transform.position.z);
            }
        }

        public void GoToNextDestination()
        {
            if(_currentPathDestination >= destinations.Length)
            {
                return;
            }

            StartCoroutine(MoveToDestinationCoroutine(destinations[_currentPathDestination]));
            _currentPathDestination++;
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

        private IEnumerator MoveToDestinationCoroutine(Transform destination)
        {
            Vector3 direction = (destination.position - _destinationPosition).normalized;

            bool destinationXIsBigger = destination.position.x > _destinationPosition.x;

            while (true)
            {
                _destinationPosition += direction * Time.deltaTime * walkVelocity;

                if(destinationXIsBigger != destination.position.x > _destinationPosition.x) 
                {
                    break;
                }

                yield return null;
            }

            _destinationPosition = destination.position;
        }

        private IEnumerator DelayMovementReading()
        {
            HandInfo rightHandInfo = new HandInfo()
            {
                up = _rightArmTargetReference.up,
                forward = _rightArmTargetReference.forward,
                localPosition = _rightArmTargetReference.localPosition + new Vector3(handsXCorreciton, handsYCorrection, 0),
            };
            HandInfo leftHandInfo = new HandInfo()
            {
                up = _leftArmTargetReference.up,
                forward = _leftArmTargetReference.forward,
                localPosition = _leftArmTargetReference.localPosition + new Vector3(-handsXCorreciton, handsYCorrection, 0),
            };

            Vector3 bodyReferencePosition = _bodyReference.position;
            Vector3 bodyReferenceForward = _bodyReference.forward;

            Vector3 rightArmHintPosition = _rightArmHintReference.localPosition;
            Vector3 leftArmHintPosition = _leftHandHintReference.localPosition;

            float headReferenceZTurn = _headReference.localEulerAngles.z;

            yield return new WaitForSeconds(movementDelay);

            if (_followHeadHeight)
            {
                //body Height
                float referenceBodyHeightDifference = bodyReferencePosition.y - _initialReferenceBodyHeight;
                if (automaticallySnapToGround)
                {
                    SnapToGround(referenceBodyHeightDifference);
                }
                else
                {
                    transform.position += new Vector3(0, referenceBodyHeightDifference + headYCorrection, 0);
                }
            }
            else
            {
                SnapToGround(0);
            }

            //body Rotation
            Vector3 direction = bodyReferencePosition - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
            newRotation.x = transform.rotation.x;
            newRotation.z = transform.rotation.z; 
            transform.rotation = newRotation;

            leftArmHint.localPosition = Vector3.Scale(rightArmHintPosition, new Vector3(-1, 1, 1));
            rightArmHint.localPosition = Vector3.Scale(leftArmHintPosition, new Vector3(-1, 1, 1));

            headTarget.localEulerAngles = new Vector3(headTarget.localEulerAngles.x, headTarget.localEulerAngles.y, headReferenceZTurn * -1);

            SetHandsRotationAndPosition(rightHandInfo, leftHandInfo, bodyReferenceForward);
        }

        private IEnumerator ClipReadingCoroutine()
        {
            _playingClip = true;

            foreach (PoseInfo pose in _clipInfo.ToList())
            {
                SetMimicPose(pose);
                yield return null;
            }

            _playingClip = false;
        }

        private void SetMimicPose(PoseInfo pose)
        {
            HandInfo rightHandInfo = new HandInfo()
            {
                up = pose.rightTargetUp,
                forward = pose.rightTargetForward,
                localPosition = pose.rightTargetLocalPos + new Vector3(handsXCorreciton, handsYCorrection, 0),
            };
            HandInfo leftHandInfo = new HandInfo()
            {
                up = pose.leftTargetUp,
                forward = pose.leftTargetForward,
                localPosition = pose.leftTargetLocalPos + new Vector3(-handsXCorreciton, handsYCorrection, 0),
            };

            //Debug.Log(pose.leftTargetLocalPos);

            //body Height
            float referenceBodyHeightDelta = pose.bodyPosition.y - _initialReferenceBodyHeight;
            transform.position += new Vector3(0, referenceBodyHeightDelta, 0);
            _initialReferenceBodyHeight = pose.bodyPosition.y;

            //body Rotation
            if(_bodyReference != null)
            {
                Vector3 direction = _bodyReference.position - transform.position;
                Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
                newRotation.x = transform.rotation.x;
                newRotation.z = transform.rotation.z;
                transform.rotation = newRotation;
            }

            leftArmHint.localPosition = Vector3.Scale(pose.rightHintLocalPos, new Vector3(-1, 1, 1));
            rightArmHint.localPosition = Vector3.Scale(pose.leftHintLocalPos, new Vector3(-1, 1, 1));

            headTarget.localEulerAngles = new Vector3(headTarget.localEulerAngles.x, headTarget.localEulerAngles.y, pose.headTargetLocalEulerAngles.z * -1);

            SetHandsRotationAndPosition(rightHandInfo, leftHandInfo, _bodyReference.forward);
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

            leftHandDestination.localPosition = Vector3.Scale(rhiLp, new Vector3(-1, 1, 1));
            rightHandDestination.localPosition = Vector3.Scale(lhiLp, new Vector3(-1, 1, 1));
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
        private Vector3 RotateVectorYAxis(Vector3 originalVector, float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float rotatedX = originalVector.x * Mathf.Cos(angleRadians) - originalVector.z * Mathf.Sin(angleRadians);
            float rotatedZ = originalVector.x * Mathf.Sin(angleRadians) + originalVector.z * Mathf.Cos(angleRadians);

            return new Vector3(rotatedX, originalVector.y, rotatedZ);
        }
    }
}