using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Manager
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private bool ajustXRAndPlayerTargets;

        void Update()
        {
            if (ajustXRAndPlayerTargets)
            {
                ajustXRAndPlayerTargets = false;
            }
        }
    }
}
