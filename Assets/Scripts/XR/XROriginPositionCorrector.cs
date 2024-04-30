using Main.Events;
using UnityEngine;

namespace Main.XR
{
    public class XROriginPositionCorrector : MonoBehaviour
    {
        [SerializeField] private Transform headSpawn;
        [SerializeField] private Vector3 offset;

        private void OnEnable()
        {
            MainEventsManager.defaultHeadPosition += ReceiveInitialHeadPosition;
        }
        private void OnDisable()
        {
            MainEventsManager.defaultHeadPosition -= ReceiveInitialHeadPosition;
        }

        private void ReceiveInitialHeadPosition(Vector3 headPosition)
        {
            Vector3 positionDifference = headSpawn.position - headPosition;
            transform.position += positionDifference + offset;
        }
    }
}
