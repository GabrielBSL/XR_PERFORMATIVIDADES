using UnityEngine;

public class GestureReferenceEventsInvoker : MonoBehaviour
{
    [SerializeField] private Transform rightTransform;
    [SerializeField] private OldTransformSampler rightTransformSampler;

    [SerializeField] private Transform leftTransform;
    [SerializeField] private OldTransformSampler leftTransformSampler;

    [SerializeField] private Transform headTransform;
    [SerializeField] private OldTransformSampler headTransformSampler;
    
    [SerializeField] private Transform jangadaTransform;
    [SerializeField] private Transform aldeia1Transform;
    [SerializeField] private Transform aldeia2Transform;
    [SerializeField] private Transform aldeia3Transform;

    void Start()
    {
        GestureReferenceEvents.leftTransform?.Invoke(leftTransform);
        GestureReferenceEvents.leftTransformSampler?.Invoke(leftTransformSampler);
        
        GestureReferenceEvents.rightTransform?.Invoke(rightTransform);
        GestureReferenceEvents.rightTransformSampler?.Invoke(rightTransformSampler);
        
        GestureReferenceEvents.headTransform?.Invoke(headTransform);
        GestureReferenceEvents.headTransformSampler?.Invoke(headTransformSampler);
        
        GestureReferenceEvents.jangadaTransform?.Invoke(jangadaTransform);
        GestureReferenceEvents.aldeia1Transform?.Invoke(aldeia1Transform);
        GestureReferenceEvents.aldeia2Transform?.Invoke(aldeia2Transform);
        GestureReferenceEvents.aldeia3Transform?.Invoke(aldeia3Transform);

    }
}
