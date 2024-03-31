using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.XR
{
    public class XROriginHeightCorrector : MonoBehaviour
    {
        [SerializeField] private float idealHeight = 1.72f;

        private void OnEnable()
        {
            MainEventsManager.defaultHeightValue += ReceiveInitialHeight;
        }
        private void OnDisable()
        {
            MainEventsManager.defaultHeightValue -= ReceiveInitialHeight;
        }

        private void ReceiveInitialHeight(float height)
        {
            Debug.Log(height);
            float heightDifference = idealHeight - height;
            transform.position = new Vector3(transform.position.x, transform.position.y + heightDifference, transform.position.z);
        }
    }
}
