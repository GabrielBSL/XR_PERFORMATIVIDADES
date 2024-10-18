using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformSampler : MonoBehaviour
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
    [HideInInspector] public AABB aabb = new AABB();
    public Vector3 ratioVector;
    public static Color ColorLerp(Vector3 vert1, Vector3 vert2, Vector3 t)
    {
        float x = Mathf.InverseLerp(vert1.x, vert2.x, t.x);
        float y = Mathf.InverseLerp(vert1.y, vert2.y, t.y);
        float z = Mathf.InverseLerp(vert1.z, vert2.z, t.z);
        return new Color(x, y, z, 1);
    }
    
    // ================================ MONOBEHAVIOUR ================================

    void Update()
    {
        sampleQueue.Enqueue(new TransformSnapshot
        (
            trackedTransform.position,
            trackedTransform.rotation
        ));
        while (sampleQueue.Count > maxSampleCount) oldestSample = sampleQueue.Dequeue();

        aabb = new AABB();
        for(int i = 0; i < sampleQueue.Count; i += 1) aabb.GrowToInclude(sampleQueue.ElementAt(i).position);
        ratioVector = aabb.direction;
    }
    private void OnDrawGizmos()
    {
        for(int i = 0; i < sampleQueue.Count; i += 1)
        {
            TransformSnapshot current = sampleQueue.ElementAt(i);
            Gizmos.color = ColorLerp(aabb.minVert, aabb.maxVert, current.position);
            Gizmos.DrawSphere(current.position, 0.005f);
            if(i > 0) Gizmos.DrawLine(sampleQueue.ElementAt(i - 1).position, current.position);
        }

        Gizmos.color = boundingVolumeColor;
        Gizmos.DrawCube(aabb.center, aabb.diagonal);
    }
}