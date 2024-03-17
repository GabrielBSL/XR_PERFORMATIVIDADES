using Main.Events;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicReference : MonoBehaviour
    {
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform rightArmHint;
        [SerializeField] private Transform leftArmHint;
        [SerializeField] private Transform headTarget;

        // Start is called before the first frame update
        void Start()
        {
            MainEventsManager.bodyTransformUpdate?.Invoke(transform);
            MainEventsManager.rightHandTargetTransformUpdate?.Invoke(rightArmTarget);
            MainEventsManager.leftHandTargetTransformUpdate?.Invoke(leftArmTarget);
            MainEventsManager.rightHandHintTransformUpdate?.Invoke(rightArmHint);
            MainEventsManager.leftHandHintTransformUpdate?.Invoke(leftArmHint);
            MainEventsManager.headTransformUpdate?.Invoke(headTarget);
        }
    }
}
