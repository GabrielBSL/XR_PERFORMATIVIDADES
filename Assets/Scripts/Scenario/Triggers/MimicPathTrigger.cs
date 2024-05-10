using Main.Mimics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario.Triggers
{
    public class MimicPathTrigger : MonoBehaviour
    {
        [System.Serializable]
        private struct MimicTrigger
        {
            public MimicChar mimic;
            public float delay;
        }

        [SerializeField] private MimicTrigger[] mimics;
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
                StartCoroutine(MimicPathDelayCoroutine(mimics[i]));
            }
        }

        private IEnumerator MimicPathDelayCoroutine(MimicTrigger mimicTrigger)
        {
            yield return new WaitForSeconds(mimicTrigger.delay);
            mimicTrigger.mimic.GoToNextDestination();
        }
    }
}
