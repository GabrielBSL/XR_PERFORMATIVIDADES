using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Main.IK
{
    public class IKTargetFollowVRRig : MonoBehaviour
    {
        [System.Serializable]
        public struct VRMap
        {
            public Transform vrTarget;
            public Transform ikTarget;
            public Vector3 trackingPositionOffset;
            public Vector3 trackingRotationOffset;
            public void Map()
            {
                ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
                ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
            }
        }

        [Range(0, 1)]
        public float turnSmoothness = 0.1f;
        public VRMap head;
        public VRMap leftHand;
        public VRMap rightHand;

        public Vector3 headBodyPositionOffset;
        public float headBodyYawOffset;

        [Header("Config")]
        [SerializeField] private InputAction ratioAction;
        [SerializeField] private float xAxisRatioBase = .87f;
        [SerializeField] private float yAxisRatioBase = 1.24f;

        [Header("Debug")]
        [SerializeField] private bool forceRatioCalculation;

        private float _currentXRatio;
        private float _currentYRatio;

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

        private void Start()
        {
            _currentYRatio = head.vrTarget.position.y;
            _currentXRatio = rightHand.vrTarget.position.x;
        }

        private void Update()
        {
            if (forceRatioCalculation)
            {
                forceRatioCalculation = false;
                SetVRAndIKTargetPositionRation();
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = head.ikTarget.position + headBodyPositionOffset;
            float yaw = head.vrTarget.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

            Vector3 positionRatio = new Vector3(xAxisRatioBase / _currentXRatio, yAxisRatioBase / _currentYRatio, 1);
            print(positionRatio.x);

            Vector3 vrTargetPosCorrected = Vector3.Scale(head.vrTarget.position, new Vector3(1, positionRatio.y, 1));
            head.ikTarget.position = vrTargetPosCorrected + head.trackingPositionOffset;
            head.ikTarget.rotation = head.vrTarget.rotation * Quaternion.Euler(head.trackingRotationOffset);

            vrTargetPosCorrected = Vector3.Scale(leftHand.vrTarget.position, positionRatio);
            leftHand.ikTarget.position = vrTargetPosCorrected + leftHand.trackingPositionOffset;
            leftHand.ikTarget.rotation = leftHand.vrTarget.rotation * Quaternion.Euler(leftHand.trackingRotationOffset);

            vrTargetPosCorrected = Vector3.Scale(rightHand.vrTarget.position, positionRatio);
            rightHand.ikTarget.position = vrTargetPosCorrected + rightHand.trackingPositionOffset;
            rightHand.ikTarget.rotation = rightHand.vrTarget.rotation * Quaternion.Euler(rightHand.trackingRotationOffset);
        }

        private void SetVRAndIKTargetPositionRation()
        {
            _currentYRatio = head.vrTarget.position.y;
            _currentXRatio = rightHand.vrTarget.position.x;
        }
    }
}
