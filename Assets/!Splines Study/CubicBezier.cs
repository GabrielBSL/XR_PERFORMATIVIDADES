using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class CubicStudy : MonoBehaviour
{
    [SerializeField] Transform A, B, C, D;
    [SerializeField] Transform target;
    [SerializeField] bool lookAtTangent;
    [SerializeField] [Range(0f, 1f)] float t;

    private Vector3 A_BLerp;
    private Vector3 B_CLerp;
    private Vector3 C_DLerp;
    private Vector3 AB_BCLerp;
    private Vector3 BC_CDLerp;
    private Vector3 ABC_BCDLerp;
    private Quaternion forward;

    private void Update()
    {
        A_BLerp = Vector3.Lerp(A.position, B.position, t);
        B_CLerp = Vector3.Lerp(B.position, C.position, t);
        C_DLerp = Vector3.Lerp(C.position, D.position, t);

        AB_BCLerp = Vector3.Lerp(A_BLerp, B_CLerp, t);
        BC_CDLerp = Vector3.Lerp(B_CLerp, C_DLerp, t);

        ABC_BCDLerp = Vector3.Lerp(AB_BCLerp, BC_CDLerp, t);

        target.position = ABC_BCDLerp;

        if(lookAtTangent)
        {
            target.rotation = Quaternion.LookRotation(BernsteinDerivative()) * forward;
        }
        else forward = target.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(A.position, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(B.position, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(C.position, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawCube(D.position, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawLine(A.position, B.position);
        Gizmos.DrawLine(B.position, C.position);
        Gizmos.DrawLine(C.position, D.position);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(A_BLerp, 0.025f);
        Gizmos.DrawSphere(B_CLerp, 0.025f);
        Gizmos.DrawSphere(C_DLerp, 0.025f);
        Gizmos.DrawLine(A_BLerp, B_CLerp);
        Gizmos.DrawLine(B_CLerp, C_DLerp);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(AB_BCLerp, 0.025f);
        Gizmos.DrawSphere(BC_CDLerp, 0.025f);
        Gizmos.DrawLine(AB_BCLerp, BC_CDLerp);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(ABC_BCDLerp, 0.025f);
    }

    private Vector3 Bernstein()
    {
        return
        (
            A.position * (Mathf.Pow(-t, 3) + Mathf.Pow(3*t, 2) - 3*t + 1) +
            B.position * (Mathf.Pow(3*t, 3) + Mathf.Pow(-6*t, 2) + 3*t) +
            C.position * (Mathf.Pow(-3*t, 3) + Mathf.Pow(3*t, 2)) +
            D.position * Mathf.Pow(t, 3)
        ).normalized;
    }
    private Vector3 BernsteinDerivative()
    {
        return
        (
            A.position * (-3 * Mathf.Pow(t, 2) + 6 * t - 3) +
            B.position * (9 * Mathf.Pow(t, 2) - 12 * t + 3) +
            C.position * (-9 * Mathf.Pow(t, 2) + 6 * t) +
            D.position * (3 * Mathf.Pow(t, 2))
        ).normalized;
    }
}