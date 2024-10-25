using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    //================ SINGLETON LOGIC ================
        public static DayNightManager instance = null;
        public void SingletonCheck()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    
    [SerializeField] [Range(0f, 1f)] public float time;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient ambientLightColor;
    [SerializeField] private Gradient fogColor;

    public void Awake()
    {
        SingletonCheck();
    }
    public void Update()
    {
        skyboxMaterial.SetFloat("_time", time);
        RenderSettings.ambientLight = ambientLightColor.Evaluate(time);
        RenderSettings.fogColor = fogColor.Evaluate(time);
    }
}