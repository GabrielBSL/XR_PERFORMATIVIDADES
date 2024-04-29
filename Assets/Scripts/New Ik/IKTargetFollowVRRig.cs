using UnityEngine;
using UnityEngine.InputSystem;
using Main.Events;
using Main.Mimics;

namespace Main.IK
{
    public class IKTargetFollowVRRig : MonoBehaviour
    {
        [System.Serializable]
        public class VRMap
        {
            public Transform vrTarget;
            public Transform ikTarget;
            public Vector3 trackingPositionOffset;
            public Vector3 trackingRotationOffset;
            public Vector3 Map(Vector3 positionRatio, Vector3 offset, bool debugPrint = false)
            {
                Vector3 vrTargetPosCorrected = Vector3.Scale(vrTarget.position - offset, positionRatio);
                Vector3 originalVrPosition = vrTarget.position;

                vrTarget.position = vrTargetPosCorrected + offset;
                ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
                vrTarget.position = originalVrPosition;

                if (debugPrint)
                {
                    //print("Original: " + originalVrPosition + " - corrected: " + vrTargetPosCorrected + " - ratio: " + positionRatio);
                }

                ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
                return ikTarget.position;
            }
        }

        [Range(0, 1)]
        public float turnSmoothness = 0.1f;
        public VRMap head;
        public VRMap leftHand;
        public VRMap rightHand;

        public Vector3 headBodyPositionOffset;
        public float headBodyYawOffset;

        [Header("Controller Transforms")]
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private Transform leftControllerTransform;
        [SerializeField] private Transform rightControllerTransform;

        [Header("Knee Target Tranforms")]
        [SerializeField] private Transform leftKneeTargetTransform;
        [SerializeField] private Transform rightKneeTargetTransform;

        [Header("Config")]
        [SerializeField] private bool allowRatioCalculation;
        [SerializeField] private Transform groundPoint;
        [SerializeField] private InputAction ratioAction;
        [SerializeField] private InputAction positionCorrectionAction;

        [Header("Debug")]
        [SerializeField] private bool forceRatioCalculation;
        [SerializeField] private bool forceBodyPositionCorrection;

        private float _xAxisRatioBase = .76f;
        private float _yAxisRatioBase = 1.93f;
        private float _currentXRatio = 1;
        private float _currentYRatio = 1;
        private float _firstHeightValue = float.NegativeInfinity;
        private Vector3 _currentHeadPosition;

        private bool _firstHeightCalculated = false;

        private void Awake()
        {
            ratioAction.performed += _ => SetVRAndIKTargetPositionRation();
            positionCorrectionAction.performed += _ => CorrectBodyPosition();

            _currentXRatio = _xAxisRatioBase;
            _currentYRatio = _yAxisRatioBase;
        }

        private void OnEnable()
        {
            ratioAction.Enable();
            positionCorrectionAction.Enable();
        }
        private void OnDisable()
        {
            ratioAction.Disable();
            positionCorrectionAction.Disable();
        }

        private void Update()
        {
            if (forceBodyPositionCorrection)
            {
                forceBodyPositionCorrection = false;
                CorrectBodyPosition();
            }
        }
        
        private void CorrectBodyPosition()
        {
            //SendCurrentHeadPosition();
            MainEventsManager.activateMimic?.Invoke();
            TryGetComponent(out MimicReference mimicReference);

            mimicReference?.SendTransforms();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (forceRatioCalculation)
            {
                forceRatioCalculation = false;
                SetVRAndIKTargetPositionRation();
            }

            transform.position = head.ikTarget.position + headBodyPositionOffset;
            float yaw = head.vrTarget.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

            Vector3 positionRatio = new Vector3(_xAxisRatioBase / _currentXRatio, _yAxisRatioBase / _currentYRatio, 1);
            Vector3 headPosition = head.Map(new Vector3(1, positionRatio.y, 1), Vector3.zero, true);
            _currentHeadPosition = headPosition;

            if (_firstHeightValue == float.NegativeInfinity)
            {
                _firstHeightValue = headPosition.y;
            }
            else if(_firstHeightValue != float.NegativeInfinity && !_firstHeightCalculated) //_firstHeightValue != headPosition.y
            {
                _firstHeightCalculated = true;
                MainEventsManager.defaultHeadPosition?.Invoke(headPosition);
            }

            leftHand.Map(positionRatio, new Vector3(head.vrTarget.position.x, 0, 0));
            rightHand.Map(positionRatio, new Vector3(head.vrTarget.position.x, 0, 0));
        }

        private void SetVRAndIKTargetPositionRation()
        {
            if (!allowRatioCalculation)
            {
                return;
            }

            _currentYRatio = mainCameraTransform.position.y - groundPoint.position.y;
            _currentXRatio = Vector3.Distance(new Vector3(rightControllerTransform.localPosition.x, 1, rightControllerTransform.localPosition.z),
                new Vector3(mainCameraTransform.localPosition.x, 1, mainCameraTransform.localPosition.z));

            Debug.Log("height: " + _currentYRatio);
            Debug.Log("width: " + _currentXRatio);
        }

        private void SendCurrentHeadPosition()
        {
            MainEventsManager.defaultHeadPosition?.Invoke(_currentHeadPosition);
        }
    }
}
