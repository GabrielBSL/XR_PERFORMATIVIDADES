using Main.Mimics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario.Triggers
{
    public class MimicFillTrigger : MonoBehaviour
    {
        [SerializeField] private MimicMaterialFill[] mimics;

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
                mimics[i].ToUpdate = true;
            }
        }
    }
}