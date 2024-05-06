using Main.Events;
using UnityEngine;

namespace Main.XR
{
    public class XROriginCorrector : MonoBehaviour
    {
        [SerializeField] private Transform headSpawn;
        [SerializeField] private Vector3 offset;

        private void OnEnable()
        {
            MainEventsManager.currentHeadPosition += ReceiveCurrentHeadPosition;
            MainEventsManager.currentHeadEulerAngles += ReceiveCurrentHeadEulerAngles;
        }
        private void OnDisable()
        {
            MainEventsManager.currentHeadPosition -= ReceiveCurrentHeadPosition;
            MainEventsManager.currentHeadEulerAngles -= ReceiveCurrentHeadEulerAngles;
        }

        private void ReceiveCurrentHeadPosition(Vector3 headPosition)
        {
            Vector3 positionDifference = headSpawn.position - headPosition;
            transform.position += positionDifference + offset;
        }
        private void ReceiveCurrentHeadEulerAngles(Vector3 headEulerAngles)
        {
            transform.eulerAngles = new Vector3(0, headEulerAngles.y * -1, 0);
        }
    }
}
