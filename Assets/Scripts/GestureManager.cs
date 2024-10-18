using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    [SerializeField] TransformSampler leftTransformSampler;
    [SerializeField] TransformSampler rightTransformSampler;
    [SerializeField] TransformSampler headTransformSampler;
    private AABB union = new AABB();
    private AABB intersection = new AABB();
    [SerializeField] bool drawUnion;
    [SerializeField] bool drawIntersection;
    
    private void Update()
    {
        union = AABB.Union(leftTransformSampler.aabb, rightTransformSampler.aabb);
        intersection = AABB.Intersection(leftTransformSampler.aabb, rightTransformSampler.aabb);
        Debug.Log($"overlap volume = {intersection.volume / union.volume}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if(drawUnion) Gizmos.DrawCube(union.center, union.diagonal);
        if(drawIntersection) Gizmos.DrawCube(intersection.center, intersection.diagonal);
    }
}
