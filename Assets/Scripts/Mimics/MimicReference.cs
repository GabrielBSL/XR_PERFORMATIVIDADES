using Main.Events;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicReference : MonoBehaviour
    {
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform headTarget;

        // Start is called before the first frame update
        void Start()
        {
            MainEventsManager.bodyTransformUpdate?.Invoke(transform);
            MainEventsManager.rightHandTransformUpdate?.Invoke(rightArmTarget);
            MainEventsManager.leftHandTransformUpdate?.Invoke(leftArmTarget);
            MainEventsManager.headTransformUpdate?.Invoke(headTarget);
        }
    }
}
