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
            MainEventsManager.defaultHeightValue += ReceiveInitialHeight;
        }
        private void OnDisable()
        {
            MainEventsManager.defaultHeightValue -= ReceiveInitialHeight;
        }

        private void ReceiveInitialHeight(Vector3 headPosition)
        {
            Debug.Log(headPosition);
            Vector3 positionDifference = headSpawn.position - headPosition;
            transform.position += positionDifference + offset;
        }
    }
}
