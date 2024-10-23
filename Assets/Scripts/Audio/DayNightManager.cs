using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightManager : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float time;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient ambientLightColor;
    [SerializeField] private Gradient fogColor;

    void Update()
    {
        skyboxMaterial.SetFloat("_time", time);
        RenderSettings.ambientLight = ambientLightColor.Evaluate(time);
        RenderSettings.fogColor = fogColor.Evaluate(time);
    }
}