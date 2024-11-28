using UnityEngine;

[CreateAssetMenu]
public class LightingData : ScriptableObject
{
    public Texture2D[] direction;
    public Texture2D[] color;
    public LightmapData[] lightmapData;
}
