using UnityEngine;
using UnityEngine.InputSystem;
using Main.Events;

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

        [Header("Config")]
        [SerializeField] private bool allowRatioCalculation;
        [SerializeField] private InputAction ratioAction;

        [Header("Debug")]
        [SerializeField] private bool forceRatioCalculation;

        private float _xAxisRatioBase = 1;
        private float _yAxisRatioBase = 1;
        private float _currentXRatio = 1;
        private float _currentYRatio = 1;
        private bool _ratioBaseConfigurated = false;
        private bool _firstHeightCalculated = false;

        private void Awake()
        {
            ratioAction.performed += _ => SetVRAndIKTargetPositionRation();
        }

        private void OnEnable()
        {
            ratioAction.Enable();
        }
        private void OnDisable()
        {
            ratioAction.Disable();
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

            Vector3 positionRatio = _ratioBaseConfigurated ? new Vector3(_xAxisRatioBase / _currentXRatio, _yAxisRatioBase / _currentYRatio, 1) : Vector3.one;
            //Debug.Log("Y ratio: " + positionRatio.y + " X ratio: " + positionRatio.x);

            float currentHeight = head.Map(new Vector3(1, positionRatio.y, 1), Vector3.zero, true).y;
            if (!_firstHeightCalculated)
            {
                MainEventsManager.defaultHeightValue?.Invoke(currentHeight);
                _firstHeightCalculated = true;
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

            _currentYRatio = mainCameraTransform.localPosition.y;
            _currentXRatio = rightControllerTransform.localPosition.x - mainCameraTransform.localPosition.x;

            if (!_ratioBaseConfigurated)
            {
                _ratioBaseConfigurated = true;

                _xAxisRatioBase = _currentXRatio;
                _yAxisRatioBase = _currentYRatio;
            }
        }
    }
}
