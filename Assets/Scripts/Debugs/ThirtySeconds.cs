using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Debugs
{
    public class ThirtySeconds : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        void Awake()
        {
            TryGetComponent(out _meshRenderer);
        }

        private void OnEnable()
        {
            MainEventsManager.clipReady += ReceiveClipStatus;
        }
        private void OnDisable()
        {
            MainEventsManager.clipReady -= ReceiveClipStatus;
        }

        private void ReceiveClipStatus(bool clipFull)
        {
            _meshRenderer.enabled = clipFull;
        }
    }
}