using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text fpsDisplay;

    void Update()
    {
        fpsDisplay.text = "FPS: " + Mathf.Round(1 / Time.deltaTime);
    }
}
