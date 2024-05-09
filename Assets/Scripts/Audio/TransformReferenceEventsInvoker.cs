using UnityEngine;

public class GestureReferenceEventsInvoker : MonoBehaviour
{
    [SerializeField] private Transform rightTransform;
    [SerializeField] private TransformSampler rightTransformSampler;

    [SerializeField] private Transform leftTransform;
    [SerializeField] private TransformSampler leftTransformSampler;

    [SerializeField] private Transform headTransform;
    [SerializeField] private TransformSampler headTransformSampler;
    
    [SerializeField] private Transform jangadaTransform;

    void Start()
    {
        GestureReferenceEvents.leftTransform?.Invoke(leftTransform);
        GestureReferenceEvents.leftTransformSampler?.Invoke(leftTransformSampler);
        
        GestureReferenceEvents.rightTransform?.Invoke(rightTransform);
        GestureReferenceEvents.rightTransformSampler?.Invoke(rightTransformSampler);
        
        GestureReferenceEvents.headTransform?.Invoke(headTransform);
        GestureReferenceEvents.headTransformSampler?.Invoke(headTransformSampler);
        
        GestureReferenceEvents.jangadaTransform?.Invoke(jangadaTransform);
    }
}
