using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.IK
{
    public class PlayerCameraPointInvoker : MonoBehaviour
    {
        [SerializeField] private Transform cameraPoint;

        // Update is called once per frame
        void Update()
        {
            MainEventsManager.currentPlayerCameraPointPos?.Invoke(cameraPoint.position);
        }
    }
}