using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightManager : MonoBehaviour
{
    [SerializeField] private bool useDynamicLighting;
    [SerializeField] [Range(0f, 1f)] private float time;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Quaternion lightRotationDay;
    [SerializeField] private Quaternion lightRotationNight;
    [SerializeField] private float lightColorTemperatureDay;
    [SerializeField] private float lightColorTemperatureNight;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient fogColor;
    [SerializeField] private Gradient ambientLightColor;


    void Update()
    {
        skyboxMaterial.SetFloat("_time", time);
        RenderSettings.ambientLight = ambientLightColor.Evaluate(time);
        if(useDynamicLighting)
        {
            directionalLight.transform.rotation = Quaternion.Slerp(lightRotationDay, lightRotationNight, time);
            directionalLight.colorTemperature = Mathf.Lerp(lightColorTemperatureDay, lightColorTemperatureNight, time);
        }
        RenderSettings.fogColor = fogColor.Evaluate(time);
    }
}
