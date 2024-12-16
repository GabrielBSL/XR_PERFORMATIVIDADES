using Main.Mimics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario.Triggers
{
    public class MimicFillTrigger : MonoBehaviour
    {
        [SerializeField] private MimicMaterialFill[] mimics;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            for (int i = 0; i < mimics.Length; i++)
            {
                mimics[i].ToUpdate = true;
            }
        }
    }
}