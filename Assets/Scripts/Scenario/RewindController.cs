using Main.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scenario
{
    public struct PoseInfo
    {
        public Vector3 rightHintLocalPos;
        public Vector3 rightTargetLocalPos;
        public Vector3 rightTargetUp;
        public Vector3 rightTargetForward;

        public Vector3 leftHintLocalPos;
        public Vector3 leftTargetLocalPos;
        public Vector3 leftTargetUp;
        public Vector3 leftTargetForward;

        public Vector3 headTargetLocalPos;
        public Vector3 headTargetLocalEulerAngles;

        public Vector3 bodyPosition;
    }

    public class RewindController : MonoBehaviour
    {
        [SerializeField, Range(1, 30)] private float clipTimeFrame = 1f;
        [SerializeField] private bool allowClipOverlap;
        [SerializeField] private InputAction saveClipAction;
        [SerializeField] private InputAction triggerMimicRewind;

        [Header("Debug")]
        [SerializeField] private bool forceSaveClip;

        private static List<List<PoseInfo>> movementClips = new List<List<PoseInfo>>();
        public static Action onStartRewind;

        private Transform _userRightTargetTransform;
        private Transform _userRightHintTransform;
        private Transform _userLeftTargetTransform;
        private Transform _userLeftHintTransform;
        private Transform _userHeadTargetTransform;
        private Transform _userBodyTransform;

        private List<PoseInfo> _currentClip = new List<PoseInfo>();
        private float _clipTime;
        private bool _clipFull;
        private static PoseInfo _pose;

        private void Awake()
        {
            saveClipAction.performed += _ => SaveClip();
            triggerMimicRewind.performed += _ => { onStartRewind?.Invoke(); };
        }

        private void OnEnable()
        {
            saveClipAction.Enable();
            triggerMimicRewind.Enable();
            MainEventsManager.rightHandTargetTransformUpdate += ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate += ReceiveLeftArmTarget;
            MainEventsManager.rightHandHintTransformUpdate += ReceiveRightArmHint;
            MainEventsManager.leftHandHintTransformUpdate += ReceiveLeftArmHint;
            MainEventsManager.headTransformUpdate += ReceiveHeadReference;
            MainEventsManager.bodyTransformUpdate += ReceiveBodyReference;
        }
        private void OnDisable()
        {
            saveClipAction.Disable();
            triggerMimicRewind.Disable();
            MainEventsManager.rightHandTargetTransformUpdate -= ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate -= ReceiveLeftArmTarget;
            MainEventsManager.rightHandHintTransformUpdate -= ReceiveRightArmHint;
            MainEventsManager.leftHandHintTransformUpdate -= ReceiveLeftArmHint;
            MainEventsManager.headTransformUpdate -= ReceiveHeadReference;
            MainEventsManager.bodyTransformUpdate -= ReceiveBodyReference;
        }

        private void ReceiveRightArmTarget(Transform _rightArmTarget)
        {
            _userRightTargetTransform = _rightArmTarget;
        }
        private void ReceiveLeftArmTarget(Transform _leftArmTarget)
        {
            _userLeftTargetTransform = _leftArmTarget;
        }
        private void ReceiveRightArmHint(Transform _rightArmHint)
        {
            _userRightHintTransform = _rightArmHint;
        }
        private void ReceiveLeftArmHint(Transform _leftArmHint)
        {
            _userLeftHintTransform = _leftArmHint;
        }
        private void ReceiveHeadReference(Transform _headReference)
        {
            _userHeadTargetTransform = _headReference;
        }
        private void ReceiveBodyReference(Transform _bodyReference)
        {
            _userBodyTransform = _bodyReference;
        }

        private void Update()
        {
            RecordPlayerPosition();

            if (forceSaveClip)
            {
                forceSaveClip = false;
                SaveClip();
                onStartRewind?.Invoke();
            }
        }

        private void SaveClip()
        {
            if(!_clipFull)
            {
                Debug.LogWarning("Clip too short to be saved");
                return;
            }

            _clipFull = false;
            _clipTime = 0;

            movementClips.Add(_currentClip.ToList());
            _currentClip.Clear();
        }

        private void RecordPlayerPosition()
        {
            _pose = new PoseInfo()
            {
                rightHintLocalPos = _userRightHintTransform.localPosition,
                rightTargetLocalPos = _userRightTargetTransform.localPosition,
                rightTargetForward = _userRightTargetTransform.forward,
                rightTargetUp = _userRightTargetTransform.up,

                leftHintLocalPos = _userLeftHintTransform.localPosition,
                leftTargetLocalPos = _userLeftTargetTransform.localPosition,
                leftTargetForward = _userLeftTargetTransform.forward,
                leftTargetUp = _userLeftTargetTransform.up,

                headTargetLocalPos = _userHeadTargetTransform.localPosition,
                headTargetLocalEulerAngles = _userHeadTargetTransform.localEulerAngles,

                bodyPosition = _userBodyTransform.position,
            };

            _currentClip.Add(_pose);
            if (_clipTime > clipTimeFrame)
            {
                _currentClip.RemoveAt(0);
                _clipFull = true;
                //Debug.Log("clip full");
            }
            else
            {
                _clipTime += Time.deltaTime;
            }
        }

        public static PoseInfo GetLastPose()
        {
            return _pose;
        }

        public static List<PoseInfo> GetRandomClip()
        {
            if(movementClips.Count == 0)
            {
                Debug.LogWarning("No clip saved");
                return null;
            }

            return movementClips[UnityEngine.Random.Range(0, movementClips.Count)].ToList();
        }
    }
}
