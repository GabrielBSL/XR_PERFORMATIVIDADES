using UnityEngine;

namespace Main.IK
{
    public class SmartElbowHint : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private Transform leftHandTarget;

        [Header("Hints")]
        [SerializeField] private Transform rightArmHint;
        [SerializeField] private Transform leftArmHint;

        [Header("Config")]
        [SerializeField] private float distanceTargetHint = 2;
        [SerializeField, Range(.001f, 1)] private float hintMoveRatioPerFrame = .05f;
        [SerializeField] private float maxAbsValueXAxis = .5f;
        [SerializeField] private float maxYValueAxis = 3.25f;
        [SerializeField] private float maxZValueAxis = -.3f;

        private int _loopCounter;

        // Update is called once per frame
        void Update()
        {
            AjustHintPosition(rightHandTarget, rightArmHint, true);
            AjustHintPosition(leftHandTarget, leftArmHint, false);
        }

        //Ajust hint position
        private void AjustHintPosition(Transform targetTransform, Transform hintTranform, bool isRight)
        {
            Vector3 direction = (targetTransform.up * -1 * distanceTargetHint);
            direction.Normalize();
            
            hintTranform.position = Vector3.Lerp(hintTranform.position, targetTransform.position + direction, hintMoveRatioPerFrame);

            float clampedLocalX;
            float correctedMaxValueXAxis;

            if (isRight )
            {
                correctedMaxValueXAxis = Mathf.Max(maxAbsValueXAxis, maxAbsValueXAxis - (targetTransform.localPosition.x - maxAbsValueXAxis));
                clampedLocalX = Mathf.Max(hintTranform.localPosition.x, correctedMaxValueXAxis);
            }
            else
            {
                float invertMaxAbsValueXAxis = maxAbsValueXAxis * -1;
                correctedMaxValueXAxis = Mathf.Min(invertMaxAbsValueXAxis, invertMaxAbsValueXAxis - (targetTransform.localPosition.x - invertMaxAbsValueXAxis));
                clampedLocalX = Mathf.Min(hintTranform.localPosition.x, correctedMaxValueXAxis);
            }

            float clampedLocalY = Mathf.Min(hintTranform.localPosition.y, maxYValueAxis);
            float clampedLocalZ = Mathf.Min(hintTranform.localPosition.z, maxZValueAxis);


            hintTranform.localPosition = new Vector3(clampedLocalX, clampedLocalY, clampedLocalZ);
        }
    }
}