using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightManager : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] public float time;

    [Header("Directional Light")]
    [SerializeField] private Light directionalLight;
    [HideInInspector] public bool overrideRotation = false;
    [SerializeField] private AnimationCurve lightRotationX;
    [SerializeField] private AnimationCurve lightIntensity;
    [SerializeField] private AnimationCurve lightColorTemperature;

    [Header("Environment")]
    [SerializeField] private Material skyboxMaterialInstance;
    [SerializeField] private Gradient ambientLightColor;
    [SerializeField] private Gradient fogColor;
    [SerializeField] private AnimationCurve fogDensity;

    void Start()
    {
        skyboxMaterialInstance = new Material(RenderSettings.skybox);
    }
    
    public void Update()
    {
        directionalLight.transform.rotation = Quaternion.Euler(lightRotationX.Evaluate(time), 0, 0);
        directionalLight.colorTemperature = lightColorTemperature.Evaluate(time);
        directionalLight.intensity = lightIntensity.Evaluate(time);

        skyboxMaterialInstance.SetFloat("_time", time);
        RenderSettings.skybox = skyboxMaterialInstance;
        RenderSettings.ambientLight = ambientLightColor.Evaluate(time);
        RenderSettings.fogColor = fogColor.Evaluate(time);
        RenderSettings.fogDensity = fogDensity.Evaluate(time);
    }
}