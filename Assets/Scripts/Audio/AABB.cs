using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB
{
    public Vector3 minVert = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    public Vector3 maxVert = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
    public Vector3 diagonal {get {return maxVert - minVert;}}
    public Vector3 direction {get {return (maxVert - minVert).normalized;}}
    public Vector3 center {get {return (minVert + maxVert) / 2;}}
    public float x {get {return maxVert.x - minVert.x;}}
    public float y {get {return maxVert.y - minVert.y;}}
    public float z {get {return maxVert.z - minVert.z;}}
    public float volume {get {return x * y * z;}}

    public void GrowToInclude(Vector3 point)
    {
        minVert.x = Mathf.Min(minVert.x, point.x);
        minVert.y = Mathf.Min(minVert.y, point.y);
        minVert.z = Mathf.Min(minVert.z, point.z);

        maxVert.x = Mathf.Max(maxVert.x, point.x);
        maxVert.y = Mathf.Max(maxVert.y, point.y);
        maxVert.z = Mathf.Max(maxVert.z, point.z);
    }

    public static AABB Union(AABB a, AABB b)
    {
        AABB result = new AABB();

        result.minVert.x = Mathf.Min(a.minVert.x, b.minVert.x);
        result.minVert.y = Mathf.Min(a.minVert.y, b.minVert.y);
        result.minVert.z = Mathf.Min(a.minVert.z, b.minVert.z);

        result.maxVert.x = Mathf.Max(a.maxVert.x, b.maxVert.x);
        result.maxVert.y = Mathf.Max(a.maxVert.y, b.maxVert.y);
        result.maxVert.z = Mathf.Max(a.maxVert.z, b.maxVert.z);

        return result;
    }

    public static AABB Intersection(AABB a, AABB b)
    {
        AABB result = new AABB();

        result.maxVert.x = Mathf.Min(a.maxVert.x, b.maxVert.x);
        result.minVert.x = Mathf.Max(a.minVert.x, b.minVert.x);
        if(result.minVert.x > result.maxVert.x)
        {
            result.minVert.x = 0;
            result.maxVert.x = 0;
        }

        result.maxVert.y = Mathf.Min(a.maxVert.y, b.maxVert.y);
        result.minVert.y = Mathf.Max(a.minVert.y, b.minVert.y);
        if(result.minVert.y > result.maxVert.y)
        {
            result.minVert.y = 0;
            result.maxVert.y = 0;
        }

        result.maxVert.z = Mathf.Min(a.maxVert.z, b.maxVert.z);
        result.minVert.z = Mathf.Max(a.minVert.z, b.minVert.z);
        if(result.minVert.z > result.maxVert.z)
        {
            result.minVert.z = 0;
            result.maxVert.z = 0;
        }

        return result;
    }

    public static float IntersectionOverUnion(AABB a, AABB b)
    {
        return Intersection(a, b).volume / Union(a, b).volume;
    }
}