using UnityEngine;

public class CenteredProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillAreaRectTransform;
    [Range(0f, 1f)] public float value = 1f;
    private void Update()
    {
        fillAreaRectTransform.sizeDelta = new Vector2
        (
            value,
            fillAreaRectTransform.sizeDelta.y
        );
    }
}