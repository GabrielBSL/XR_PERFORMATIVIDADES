using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MyImageEffect : MonoBehaviour
{
    [SerializeField] private Shader shader;
    private Material material;

    private void Awake()
    {
        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, material);
    }
}