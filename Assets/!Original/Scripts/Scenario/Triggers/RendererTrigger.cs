using Main.Mimics;
using UnityEngine;

namespace Main.Scenario.Triggers
{
    public class RendererTrigger : MonoBehaviour
    {
        [SerializeField] private MimicChar[] mimics;
        [SerializeField] private bool setEnabled = true;

        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _triggered)
            {
                return;
            }

            _triggered = true;

            for (int i = 0; i < mimics.Length; i++)
            {
                mimics[i].MeshRenderer.enabled = setEnabled;
            }
        }
    }
}