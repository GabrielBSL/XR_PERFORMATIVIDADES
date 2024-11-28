using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapBlender : MonoBehaviour
{
    public ComputeShader blendShader;
    public LightingData lightingDataA;
    public LightingData lightingDataB;
    [Range(0f, 1f)] public float t;
    public int frequency = 10;

    public Texture2D BlendTextures(Texture2D a, Texture2D b, float t)
    {
        int width = a.width;
        int height = b.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Set textures and parameters in the compute shader
        int kernelHandle = blendShader.FindKernel("CSMain");
        blendShader.SetTexture(kernelHandle, "_Texture1", a);
        blendShader.SetTexture(kernelHandle, "_Texture2", b);
        blendShader.SetTexture(kernelHandle, "_ResultTexture", renderTexture);
        blendShader.SetFloat("_BlendFactor", t);

        // Dispatch the shader to process in blocks of 8x8 threads
        blendShader.Dispatch(kernelHandle, Mathf.CeilToInt(width / 8.0f), Mathf.CeilToInt(height / 8.0f), 1);

        // Convert RenderTexture to Texture2D
        Texture2D outputTexture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = renderTexture;
        outputTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        outputTexture.Apply();
        RenderTexture.active = null;
        
        // Clean up
        renderTexture.Release();
        
        return outputTexture;
    }

    public void ReloadLightmap()
    {        
        int colorCount = lightingDataA.color.Length;
        if(colorCount != lightingDataB.color.Length)
        {
            Debug.Log("Lightmap color texture count mismatched");
            return;
        }

        int directionCount = lightingDataA.direction.Length;
        if(directionCount != lightingDataB.direction.Length)
        {
            Debug.Log("Lightmap direction texture count mismatched");
            return;
        }

        LightmapData[] blendedLightmap = new LightmapData[colorCount];
        for(int i = 0; i < colorCount; i += 1)
        {
            LightmapData lightmapData = new LightmapData();
            lightmapData.lightmapColor = BlendTextures(lightingDataA.color[i], lightingDataB.color[i], t);
            lightmapData.lightmapDir = BlendTextures(lightingDataA.direction[i], lightingDataB.direction[i], t);
            blendedLightmap[i] = lightmapData;
        }
        LightmapSettings.lightmaps = blendedLightmap;
    }

    private IEnumerator ReloadLightmapCoroutine()
    {
        while(true)
        {
            ReloadLightmap();
            for(int i = 0; i < frequency - 1; i += 1) yield return null;
            yield return null;
        }
    }

    public void Start()
    {
        StartCoroutine(ReloadLightmapCoroutine());
    }
    
}
