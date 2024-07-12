using UnityEngine;
using UnityEngine.InputSystem;
using Main.Events;
using Main.Mimics;
using System;

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

        [Header("Legs Tranforms")]
        [SerializeField] private IKFootSolver[] footSolvers;
        [SerializeField] private Transform leftKneeTargetTransform;
        [SerializeField] private Transform rightKneeTargetTransform;

        [Header("Ground Transform")]
        [SerializeField] private Transform groundPoint;

        [Header("Config")]
        [SerializeField] private bool allowRatioCalculation;
        [SerializeField] private bool allowInitialPositionCorrection = true;
        [SerializeField] private float minimalHeadDistanceToAutoPosCorrect = .5f;
        [SerializeField] private InputAction ratioAction;
        [SerializeField] private IKFootSolver[] footSolver;
        [SerializeField] private Camera playerCamera;

        [Header("Origin Correction")]
        [SerializeField] private bool correctPosition;
        [SerializeField] private bool correctRotation;
        [SerializeField] private InputAction originCorrectionAction;

        [Header("Debug")]
        [SerializeField] private bool forceRatioCalculation;
        [SerializeField] private bool forceBodyPositionCorrection;

        private float _vrikRatioTimer = 0;
        private float _correctBodyPosTimer = 0;
        private float _xAxisRatioBase = .63f;
        private float _yAxisRatioBase = 2.18f;
        private float _currentXRatio = 1;
        private float _currentYRatio = 1;
        private float _firstHeightValue = float.NegativeInfinity;
        private Vector3 _currentHeadPosition;

        private bool _firstHeightCalculated = false;
        private bool _vrikRatioButtonPressed;
        private bool _correctBodyPosButtonPressed;

        private void Awake()
        {
            ratioAction.performed += _ => ReceiveVRIKButtonUpdate(true);
            ratioAction.canceled += _ => ReceiveVRIKButtonUpdate(false);
            originCorrectionAction.performed += _ => ReceiveCorrectBodyPositionButtonUpdate(true);
            originCorrectionAction.canceled += _ => ReceiveCorrectBodyPositionButtonUpdate(false);
        }

        private void OnEnable()
        {
            ratioAction.Enable();
            originCorrectionAction.Enable();
            MainEventsManager.pathStartMoving += ReceiveStartPath;
            MainEventsManager.pathReachingEnd += ReceivePathEnding;
        }

        private void OnDisable()
        {
            ratioAction.Disable();
            originCorrectionAction.Disable();
            MainEventsManager.pathStartMoving -= ReceiveStartPath;
            MainEventsManager.pathReachingEnd -= ReceivePathEnding;
        }

        private void ReceiveStartPath()
        {
            for (int i = 0; i < footSolver.Length; i++)
            {
                footSolver[i].enabled = false;
            }
        }
        private void ReceivePathEnding()
        {
            for (int i = 0; i < footSolver.Length; i++)
            {
                footSolver[i].enabled = true;
            }
        }

        private void Start()
        {
            _currentXRatio = _xAxisRatioBase;
            _currentYRatio = _yAxisRatioBase;

            Vector3 positionRatio = new Vector3(_xAxisRatioBase / _currentXRatio, _yAxisRatioBase / _currentYRatio, 1);
            Vector3 headPosition = head.Map(new Vector3(1, positionRatio.y, 1), Vector3.zero, true);
            _currentHeadPosition = headPosition;

            MainEventsManager.onPlayerCamera?.Invoke(playerCamera);
        }

        private void ReceiveVRIKButtonUpdate(bool isPressed)
        {
            _vrikRatioButtonPressed = isPressed;
        }
        private void ReceiveCorrectBodyPositionButtonUpdate(bool isPressed)
        {
            _correctBodyPosButtonPressed = isPressed;
        }

        private void Update()
        {
            if (forceBodyPositionCorrection)
            {
                forceBodyPositionCorrection = false;
                CorrectBodyPosition();
            }

            _correctBodyPosTimer = _correctBodyPosButtonPressed ? _correctBodyPosTimer + Time.deltaTime : 0;
            _vrikRatioTimer = _vrikRatioButtonPressed ? _vrikRatioTimer + Time.deltaTime : 0;

            if( _correctBodyPosTimer > 2) 
            {
                _correctBodyPosButtonPressed = false;
                _correctBodyPosTimer = 0;
                CorrectBodyPosition();
            }
            if (_vrikRatioTimer > 2)
            {
                _vrikRatioButtonPressed = false;
                _vrikRatioTimer = 0;
                SetVRAndIKTargetPositionRation();
            }
        }

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

            if((headPosition - _currentHeadPosition).sqrMagnitude > Mathf.Pow(minimalHeadDistanceToAutoPosCorrect, 2))
            {
                MainEventsManager.currentHeadPosition?.Invoke(headPosition);
            }

            _currentHeadPosition = headPosition;

            if (_firstHeightValue == float.NegativeInfinity)
            {
                _firstHeightValue = headPosition.y;
            }
            else if(_firstHeightValue != float.NegativeInfinity && !_firstHeightCalculated && allowInitialPositionCorrection)
            {
                _firstHeightCalculated = true;
                MainEventsManager.currentHeadPosition?.Invoke(headPosition);
            }

            leftHand.Map(positionRatio, new Vector3(head.vrTarget.position.x, 0, 0));
            rightHand.Map(positionRatio, new Vector3(head.vrTarget.position.x, 0, 0));
        }

        private void CorrectBodyPosition()
        {
            if (correctPosition)
            {
                SendCurrentHeadPosition();
            }
            if (correctRotation)
            {
                SendCurrentHeadRotation();
            }
            MainEventsManager.activateMimic?.Invoke();
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

            GestureReferenceEvents.userHeight?.Invoke(_currentYRatio);
            GestureReferenceEvents.userWingspan?.Invoke(_currentXRatio);
        }

        private void SendCurrentHeadPosition()
        {
            MainEventsManager.currentHeadPosition?.Invoke(_currentHeadPosition);
        }
        private void SendCurrentHeadRotation()
        {
            MainEventsManager.currentHeadEulerAngles?.Invoke(transform.eulerAngles);
        }
    }
}

/*
 * Eu estou trabalhando em um projeto 3D para Unity e estou precisando de ajuda.

Eu tenho uma tabela com 100 entradas, cada entrada possui os valores de posição e rotação da cabeça e das mãos de um modelo, cada posição e rotação possui valore x, y e z, e cada entrada possui esses valores para cada segundo por 10 segundos e, ao final, teria um valor que classificaria o tal movimento. Ou seja, cada entrada possui a posição e rotação da cabeça e das duas mãos em 10 tempos distintos para uma classificação (6 x 3 x 10 + 1), contabilizando 181 entradas.

Essa tabela está sendo representada em um array bidimensional [100][181] de floats. Minha ideia é que esses valores sejam usados por um algoritmo de aprendizado de máquina para que, quando uma nova entrada chegar (um novo modelo fazendo movimentos distintos), eu possa identificar qual classificação dar para esse movimento.

Qual seria o melhor algoritmo para esse caso em específico? Considere que o valor classificador, por mais que seja float, terá apenas valores inteiros maiores ou iguais a 0.
*/