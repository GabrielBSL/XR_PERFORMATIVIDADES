using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class BezierStudy : MonoBehaviour
{
    [SerializeField] Transform A, B, C;
    [SerializeField] Transform target;
    [SerializeField] bool lookAtTangent;
    [SerializeField] [Range(0f, 1f)] float t;

    private Vector3 ABLerp;
    private Vector3 BCLerp;
    private Vector3 ABCLerp;
    private Quaternion forward;

    private void Update()
    {
        ABLerp = Vector3.Lerp(A.position, B.position, t);
        BCLerp = Vector3.Lerp(B.position, C.position, t);
        ABCLerp = Vector3.Lerp(ABLerp, BCLerp, t);

        target.position = ABCLerp;
        if(lookAtTangent)
        {
            target.rotation = Quaternion.LookRotation(BernsteinDerivative()) * forward;
        }
        else
        {
            forward = target.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(A.position, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(C.position, new Vector3(0.05f, 0.05f, 0.05f));
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(B.position, 0.05f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(A.position, B.position);
        Gizmos.DrawLine(B.position, C.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(ABLerp, 0.025f);
        Gizmos.DrawSphere(BCLerp, 0.025f);
        Gizmos.DrawLine(ABLerp, BCLerp);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(ABCLerp, 0.05f);        
    }

    /*
    private Vector3 Bernstein()
    {
        return
        A.position * (1-t)*(1-t)+
        B.position * (2*t)*(1-t)+
        C.position * (t*t);
    }
    */

    private Vector3 BernsteinDerivative()
    {
        return (2*(B.position - A.position) + 2*t *(A.position - 2*B.position + C.position)).normalized;
    }
}