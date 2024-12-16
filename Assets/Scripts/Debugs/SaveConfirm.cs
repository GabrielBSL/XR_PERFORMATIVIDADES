using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Debugs
{
    public class SaveConfirm : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void OnEnable()
        {
            MainEventsManager.clipSaved += ReceiveSaveConfirm;
        }
        private void OnDisable()
        {
            MainEventsManager.clipSaved -= ReceiveSaveConfirm;
        }


        void Start()
        {
            TryGetComponent(out _meshRenderer);
            _meshRenderer.enabled = false;
        }

        private void ReceiveSaveConfirm()
        {
            StartCoroutine(RendererCoroutine());
        }

        private IEnumerator RendererCoroutine()
        {
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(3);
            _meshRenderer.enabled = false;
        }
    }
}