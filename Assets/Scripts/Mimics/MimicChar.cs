using Main.Events;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Main.Mimics
{
    public class MimicChar : MonoBehaviour
    {
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;

        private Transform rightArmTargetReference;
        private Transform leftArmTargetReference;
        private Transform bodyReference;

        private Vector3 _lastRightHandLocalPos;
        private Vector3 _lastLeftHandLocalPos;

        private Vector3 _lastRightHandGlobalPos;
        private Vector3 _lastLeftHandGlobalPos;

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
            rightArmTarget.localPosition = _rightArmTarget.localPosition;
            _lastRightHandLocalPos = _rightArmTarget.localPosition;
            _lastRightHandGlobalPos = _rightArmTarget.position;
        }
        private void ReceiveLeftArmTarget(Transform _leftArmTarget)
        {
            leftArmTargetReference = _leftArmTarget;
            leftArmTarget.localPosition = _leftArmTarget.localPosition;
            _lastLeftHandLocalPos = _leftArmTarget.localPosition;
            _lastLeftHandGlobalPos = _leftArmTarget.position;
        }
        private void ReceiveBodyReference(Transform _bodyReference)
        {
            bodyReference = _bodyReference;
        }

        // Update is called once per frame
        void Update()
        {
            //body Height
            transform.position = new Vector3(transform.position.x, bodyReference.position.y, transform.position.z);

            //body Rotation
            Vector3 direction = bodyReference.position - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
            newRotation.x = transform.rotation.x;
            newRotation.z = transform.rotation.z;
            transform.rotation = newRotation;

            //Hands angles
            rightArmTarget.localEulerAngles = rightArmTargetReference.localEulerAngles;
            leftArmTarget.localEulerAngles = leftArmTargetReference.localEulerAngles;

            //Hands positions
            //Right hand movements of the player are translated to left hand of the mimic for the "Mirror" effect
            Vector3 rightHandLocalDelta = rightArmTargetReference.localPosition - _lastRightHandLocalPos;
            Vector3 leftHandLocalDelta = leftArmTargetReference.localPosition - _lastLeftHandLocalPos;

            Vector3 rightHandGlobalDelta = rightArmTargetReference.position - _lastRightHandGlobalPos;
            Vector3 leftHandGlobalDelta = leftArmTargetReference.position - _lastLeftHandGlobalPos;

            Vector2 mimicFacingDir = new Vector2(transform.forward.x, transform.forward.z);
            Vector2 referenceFacingDir = new Vector2(bodyReference.forward.x, bodyReference.forward.z);

            Vector2 mimicRightHandDelta = RotateVector(referenceFacingDir, mimicFacingDir, rightHandLocalDelta, rightHandGlobalDelta);
            Vector2 mimicLeftHandDelta = RotateVector(referenceFacingDir, mimicFacingDir, leftHandLocalDelta, leftHandGlobalDelta);

            rightArmTarget.localPosition += new Vector3(mimicRightHandDelta.x, 0, mimicRightHandDelta.y) * 2;
            leftArmTarget.localPosition += new Vector3(mimicLeftHandDelta.x, 0, mimicLeftHandDelta.y) * 2;

            _lastRightHandLocalPos = rightArmTargetReference.localPosition;
            _lastLeftHandLocalPos = leftArmTargetReference.localPosition;

            _lastLeftHandGlobalPos = leftArmTargetReference.position;
            _lastRightHandGlobalPos = rightArmTargetReference.position;
        }

        public Vector2 RotateVector(Vector2 dirInitial, Vector2 dirFinal, Vector3 localDelta, Vector3 globalDelta)
        {
            Vector2 local2DDelta = new Vector2(localDelta.x, localDelta.z);

            if(local2DDelta == Vector2.zero)
            {
                return Vector2.zero;
            }

            Vector2 global2DDelta = new Vector2(globalDelta.x, globalDelta.z);

            // Calcula o ângulo de rotação entre dirInitial e dirFinal
            float angle = Vector2.SignedAngle(dirInitial, dirFinal);

            // Rotaciona dirProduct pelo ângulo calculado
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 rotatedDirProduct = rotation * global2DDelta;

            return rotatedDirProduct;
        }
    }
}