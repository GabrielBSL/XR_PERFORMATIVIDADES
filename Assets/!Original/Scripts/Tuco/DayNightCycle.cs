using UnityEngine;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] public float time;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient ambientLightColor;
    [SerializeField] private Gradient fogColor;

    public void Update()
    {
        skyboxMaterial.SetFloat("_time", time);
        RenderSettings.ambientLight = ambientLightColor.Evaluate(time);
        RenderSettings.fogColor = fogColor.Evaluate(time);
    }
}