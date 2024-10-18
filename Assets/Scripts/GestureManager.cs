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
        float dot = Vector3.Dot(leftTransformSampler.aabb.direction, rightTransformSampler.aabb.direction);
        Debug.Log($"dot = {dot}");

        union = AABB.Union(leftTransformSampler.aabb, rightTransformSampler.aabb);
        intersection = AABB.Intersection(leftTransformSampler.aabb, rightTransformSampler.aabb);
        Debug.Log($"overlap volume = {intersection.volume / union.volume}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(drawUnion) Gizmos.DrawCube(union.center, union.diagonal);
        Gizmos.color = Color.black;
        if(drawIntersection) Gizmos.DrawCube(intersection.center, intersection.diagonal);
    }
}
