using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario.Triggers
{
    public class RendererTrigger : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private bool setEnabled = true;

        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _triggered)
            {
                return;
            }

            _triggered = true;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = setEnabled;
            }
        }
    }
}
