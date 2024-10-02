using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Transform trackedTransform;
    [SerializeField] private Color boundingVolumeColor;

    class TransformSnapshot
    {
        public Vector3 position;
        public Quaternion rotation;
        public TransformSnapshot(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    [SerializeField] private Queue<TransformSnapshot> sampleQueue = new Queue<TransformSnapshot>();
    [SerializeField] private int maxSampleCount = 8;
    private TransformSnapshot oldestSample;
    private Vector3 minVert = new Vector3();
    private Vector3 maxVert = new Vector3();
    public static Color ColorLerp(Vector3 vert1, Vector3 vert2, Vector3 t)
    {
        float x = Mathf.InverseLerp(vert1.x, vert2.x, t.x);
        float y = Mathf.InverseLerp(vert1.y, vert2.y, t.y);
        float z = Mathf.InverseLerp(vert1.z, vert2.z, t.z);
        return new Color(x, y, z, 1);
    }
    private void GrowToInclude(Vector3 point)
    {
        if(point.x < minVert.x) minVert.x = point.x;
        if(point.y < minVert.y) minVert.y = point.y;
        if(point.z < minVert.z) minVert.z = point.z;

        if(point.x > maxVert.x) maxVert.x = point.x;
        if(point.y > maxVert.y) maxVert.y = point.y;
        if(point.z > maxVert.z) maxVert.z = point.z;
    }

    void Update()
    {
        sampleQueue.Enqueue(new TransformSnapshot
        (
            trackedTransform.position,
            trackedTransform.rotation
        ));
        while (sampleQueue.Count > maxSampleCount) oldestSample = sampleQueue.Dequeue();
    }
    private void OnDrawGizmos()
    {
        minVert = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        maxVert = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
        Vector3 ratioVector = (maxVert - minVert).normalized;

        for(int i = 0; i < sampleQueue.Count; i += 1)
        {
            TransformSnapshot current = sampleQueue.ElementAt(i);
            GrowToInclude(current.position);

            Gizmos.color = ColorLerp(minVert, maxVert, current.position);
            Gizmos.DrawSphere(current.position, 0.005f);
            if(i > 0) Gizmos.DrawLine(sampleQueue.ElementAt(i - 1).position, current.position);
        }

        Gizmos.color = boundingVolumeColor;
        Gizmos.DrawCube((minVert + maxVert) / 2, maxVert - minVert);
        Debug.Log($"Ratio = {ratioVector}");
    }
}