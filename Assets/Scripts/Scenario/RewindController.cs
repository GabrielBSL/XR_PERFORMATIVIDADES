using Main.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

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

    [Serializable]
    public struct PoseJson
    {
        public float[] rightHintLocalPos;
        public float[] rightTargetLocalPos;
        public float[] rightTargetUp;
        public float[] rightTargetForward;

        public float[] leftHintLocalPos;
        public float[] leftTargetLocalPos;
        public float[] leftTargetUp;
        public float[] leftTargetForward;

        public float[] headTargetLocalPos;
        public float[] headTargetLocalEulerAngles;

        public float[] bodyPosition;
    }

    [Serializable]
    public class Clip
    {
        public PoseJson[] poses;
    }

    [Serializable]
    public class ClipsHolder
    {
        public string clipName;
        public Clip[] clipsArray;
    }

    public class RewindController : MonoBehaviour
    {
        [SerializeField, Range(1, 30)] private float clipTimeFrame = 1f;
        [SerializeField] private bool allowClipOverlap;
        [SerializeField] private InputAction saveClipAction;
        [SerializeField] private InputAction triggerMimicRewind;

        [Header("Debug")]
        [SerializeField] private bool forceSaveClip;
        [SerializeField] private bool forceRewind;

        private static List<List<PoseInfo>> _movementClips = new List<List<PoseInfo>>();
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

        private float _saveClipPressTime = 0;
        private float _startRewindPressTime = 0;

        private bool _saveClipPressed;
        private bool _startRewindPressed;

        public static bool ClipsLoaded { get; private set; }

        private string _clipsPath;

        private void Awake()
        {
            _clipsPath = Application.persistentDataPath + "/clips.json";

            saveClipAction.performed += _ => ReceiveSaveClipButtonUpdate(true);
            saveClipAction.canceled += _ => ReceiveSaveClipButtonUpdate(false);
            triggerMimicRewind.performed += _ => ReceiveStartRewindButtonUpdate(true);
            triggerMimicRewind.canceled += _ => ReceiveStartRewindButtonUpdate(false);

            _movementClips = LoadClipsFromFile();
            ClipsLoaded = true;
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

            _startRewindPressTime = _startRewindPressed ? _startRewindPressTime + Time.deltaTime : 0;
            _saveClipPressTime = _saveClipPressed ? _saveClipPressTime + Time.deltaTime : 0;

            if(_startRewindPressTime > 1)
            {
                _startRewindPressed = false;
                _startRewindPressTime = 0;
                onStartRewind?.Invoke();
            }
            if(_saveClipPressTime > 1)
            {
                _saveClipPressTime = 0;
                _saveClipPressed = false;
                SaveClip();
            }

            if (_startRewindPressed)
            {
                _startRewindPressTime += Time.deltaTime;
            }
            if (_saveClipPressed)
            {
                _saveClipPressTime += Time.deltaTime;
            }

            if (forceSaveClip)
            {
                forceSaveClip = false;
                SaveClip();
            }

            if (forceRewind)
            {
                forceRewind = false;
                onStartRewind?.Invoke();
            }
        }

        private void ReceiveSaveClipButtonUpdate(bool isPressed)
        {
            _saveClipPressed = isPressed;
        }
        private void ReceiveStartRewindButtonUpdate(bool isPressed)
        {
            //_startRewindPressed = isPressed;
        }

        private List<List<PoseInfo>> LoadClipsFromFile()
        {
            if(!File.Exists(_clipsPath))
            {
                return new List<List<PoseInfo>>();
            }

            string jsonFile = File.ReadAllText(_clipsPath);
            ClipsHolder clips = JsonUtility.FromJson<ClipsHolder>(jsonFile);

            return ConvertPoseClipToPoseInfo(clips);
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

            _movementClips.Add(_currentClip.ToList());
            WriteClipInFile();
            _currentClip.Clear();

            MainEventsManager.clipSaved?.Invoke();
        }

        private void WriteClipInFile()
        {
            Clip[] newClipsArray = ConvertPoseInfoToPoseJson();
            ClipsHolder clipsHolder = new ClipsHolder() { clipsArray = newClipsArray, clipName = "ClipNumber" + _movementClips.Count};

            string json = JsonUtility.ToJson(clipsHolder);
            File.WriteAllText(_clipsPath, json);
        }

        private List<List<PoseInfo>> ConvertPoseClipToPoseInfo(ClipsHolder clips)
        {
            var newPoseList = new List<List<PoseInfo>>();

            foreach (var fileClip in clips.clipsArray)
            {
                newPoseList.Add(new List<PoseInfo>());

                foreach (var pose in fileClip.poses)
                {
                    PoseInfo newPoseJson = new PoseInfo()
                    {
                        rightHintLocalPos = new Vector3(pose.rightHintLocalPos[0], pose.rightHintLocalPos[1], pose.rightHintLocalPos[2]),
                        rightTargetLocalPos = new Vector3(pose.rightTargetLocalPos[0], pose.rightTargetLocalPos[1], pose.rightTargetLocalPos[2]),
                        rightTargetUp = new Vector3(pose.rightTargetUp[0], pose.rightTargetUp[1], pose.rightTargetUp[2]),
                        rightTargetForward = new Vector3(pose.rightTargetForward[0], pose.rightTargetForward[1], pose.rightTargetForward[2]),

                        leftHintLocalPos = new Vector3(pose.leftHintLocalPos[0], pose.leftHintLocalPos[1], pose.leftHintLocalPos[2]),
                        leftTargetLocalPos = new Vector3(pose.leftTargetLocalPos[0], pose.leftTargetLocalPos[1], pose.leftTargetLocalPos[2]),
                        leftTargetUp = new Vector3(pose.leftTargetUp[0], pose.leftTargetUp[1], pose.leftTargetUp[2]),
                        leftTargetForward = new Vector3(pose.leftTargetForward[0], pose.leftTargetForward[1], pose.leftTargetForward[2]),

                        headTargetLocalPos = new Vector3(pose.headTargetLocalPos[0], pose.headTargetLocalPos[1], pose.headTargetLocalPos[2]),
                        headTargetLocalEulerAngles = new Vector3(pose.headTargetLocalEulerAngles[0], pose.headTargetLocalEulerAngles[1], pose.headTargetLocalEulerAngles[2]),

                        bodyPosition = new Vector3(pose.bodyPosition[0], pose.bodyPosition[1], pose.bodyPosition[2]),
                    };

                    newPoseList[newPoseList.Count - 1].Add(newPoseJson);
                }
            }

            return newPoseList;
        }

        private Clip[] ConvertPoseInfoToPoseJson()
        {
            Clip[] newClipArray = new Clip[_movementClips.Count];

            int clipArrayIndex = 0;
            foreach (var savedClip in _movementClips)
            {
                newClipArray[clipArrayIndex] = new Clip() { poses = new PoseJson[savedClip.Count] };
                int clipIndex = 0;

                foreach (var pose in savedClip)
                {
                    PoseJson newPoseJson = new PoseJson()
                    {
                        rightHintLocalPos = new float[3] { pose.rightHintLocalPos.x, pose.rightHintLocalPos.y, pose.rightHintLocalPos.z },
                        rightTargetLocalPos = new float[3] {pose.rightTargetLocalPos.x, pose.rightTargetLocalPos.y, pose.rightTargetLocalPos.z },
                        rightTargetUp = new float[3] {pose.rightTargetUp.x, pose.rightTargetUp.y, pose.rightTargetUp.z },
                        rightTargetForward = new float[3] {pose.rightTargetForward.x, pose.rightTargetForward.y, pose.rightTargetForward.z },

                        leftHintLocalPos = new float[3] { pose.leftHintLocalPos.x, pose.leftHintLocalPos.y, pose.leftHintLocalPos.z },
                        leftTargetLocalPos = new float[3] { pose.leftTargetLocalPos.x, pose.leftTargetLocalPos.y, pose.leftTargetLocalPos.z },
                        leftTargetUp = new float[3] { pose.leftTargetUp.x, pose.leftTargetUp.y, pose.leftTargetUp.z },
                        leftTargetForward = new float[3] { pose.leftTargetForward.x, pose.leftTargetForward.y, pose.leftTargetForward.z },

                        headTargetLocalPos = new float[3] {pose.headTargetLocalPos.x, pose.headTargetLocalPos.y, pose.headTargetLocalPos.z },
                        headTargetLocalEulerAngles = new float[3] {pose.headTargetLocalEulerAngles.x,  pose.headTargetLocalEulerAngles.y, pose.headTargetLocalEulerAngles.z },

                        bodyPosition = new float[3] {pose.bodyPosition.x, pose.bodyPosition.y, pose.bodyPosition.z },
                    };

                    newClipArray[clipArrayIndex].poses[clipIndex] = newPoseJson;
                    clipIndex++;
                }

                clipArrayIndex++;
            }

            return newClipArray;
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
            }
            else
            {
                _clipTime += Time.deltaTime;
            }

            MainEventsManager.clipReady?.Invoke(_clipFull);
        }

        public static PoseInfo GetLastPose()
        {
            return _pose;
        }

        public static List<PoseInfo> GetRandomClip()
        {
            if(_movementClips.Count == 0)
            {
                Debug.LogWarning("No clip saved");
                return null;
            }

            return _movementClips[UnityEngine.Random.Range(0, _movementClips.Count)].ToList();
        }

        public static PoseInfo PoseLerp(PoseInfo a, PoseInfo b, float t)
        {
            PoseInfo lerp = new PoseInfo()
            {
                rightHintLocalPos = Vector3.Lerp(a.rightHintLocalPos, b.rightHintLocalPos, t),
                rightTargetLocalPos = Vector3.Lerp(a.rightTargetLocalPos, b.rightTargetLocalPos, t),
                rightTargetUp = Vector3.Lerp(a.rightTargetUp, b.rightTargetUp, t),
                rightTargetForward = Vector3.Lerp(a.rightTargetForward, b.rightTargetForward, t),

                leftHintLocalPos = Vector3.Lerp(a.leftHintLocalPos, b.leftHintLocalPos, t),
                leftTargetLocalPos = Vector3.Lerp(a.leftTargetLocalPos, b.leftTargetLocalPos, t),
                leftTargetUp = Vector3.Lerp(a.leftTargetUp, b.leftTargetUp, t),
                leftTargetForward = Vector3.Lerp(a.leftTargetForward, b.leftTargetForward, t),

                headTargetLocalPos = Vector3.Lerp(a.headTargetLocalPos, b.headTargetLocalPos, t),
                headTargetLocalEulerAngles = Vector3.Lerp(a.headTargetLocalEulerAngles, b.headTargetLocalEulerAngles, t),

                bodyPosition = Vector3.Lerp(a.bodyPosition, b.bodyPosition, t),
            };

            return lerp;
        }
    }
}
