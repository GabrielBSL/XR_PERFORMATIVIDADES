using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightManager : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float time;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Quaternion lightRotationDay;
    [SerializeField] private Quaternion lightRotationNight;
    [SerializeField] private float lightColorTemperatureDay;
    [SerializeField] private float lightColorTemperatureNight;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Color fogColorDay;
    [SerializeField] private Color fogColorNight;

    void Update()
    {
        skyboxMaterial.SetFloat("_time", time);
        directionalLight.transform.rotation = Quaternion.Slerp(lightRotationDay, lightRotationNight, time);
        directionalLight.colorTemperature = Mathf.Lerp(lightColorTemperatureDay, lightColorTemperatureNight, time);
        RenderSettings.fogColor = Color.Lerp(fogColorDay, fogColorNight, time);
    }
}
