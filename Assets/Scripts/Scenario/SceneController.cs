using Main.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameObject mimicToActivate;

        private void OnEnable()
        {
            MainEventsManager.activateMimic += ActivateMimic;
        }
        private void OnDisable()
        {

            MainEventsManager.activateMimic -= ActivateMimic;
        }

        private void ActivateMimic()
        {
            mimicToActivate.SetActive(true);
        }
    }
}
