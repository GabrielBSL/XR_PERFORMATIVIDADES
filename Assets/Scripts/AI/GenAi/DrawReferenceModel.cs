using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

[ExecuteInEditMode]
public class DrawReferenceModel : MonoBehaviour
{
    public float height = 1.8f;
    public float gizmoRadius = 1f;
    public Transform modelRoot;
    public TextAsset motionText;
    public bool setTransforms;

    [Header("Limit angles")]
    [Header("X is Z, Y is X, Z is Y")]
    public Vector3 angleBpMin;
    public Vector3 angleBpMax;
    public Vector3 angleBtMin;
    public Vector3 angleBtMax;
    public Vector3 angleBlnMin;
    public Vector3 angleBlnMax;
    public Vector3 angleBunMin;
    public Vector3 angleBunMax;
    public Vector3 angleLsMin;
    public Vector3 angleLsMax;
    public Vector3 angleLeMin;
    public Vector3 angleLeMax;
    public Vector3 angleLwMin;
    public Vector3 angleLwMax;
    public Vector3 angleRsMin;
    public Vector3 angleRsMax;
    public Vector3 angleReMin;
    public Vector3 angleReMax;
    public Vector3 angleRwMin;
    public Vector3 angleRwMax;
    public Vector3 angleLhMin;
    public Vector3 angleLhMax;
    public Vector3 angleLkMin;
    public Vector3 angleLkMax;
    public Vector3 angleLaMin;
    public Vector3 angleLaMax;
    public Vector3 angleRhMin;
    public Vector3 angleRhMax;
    public Vector3 angleRkMin;
    public Vector3 angleRkMax;
    public Vector3 angleRaMin;
    public Vector3 angleRaMax;

    [ExecuteInEditMode]
    void Update()
    {
        if (setTransforms)
        {
            SetJoints();
            MoveJoints();
        }
    }

    private void SetJoints()
    {
        Transform bp = modelRoot.GetChild(0);
        Transform bt = bp.GetChild(0);
        Transform bln = bt.GetChild(0);
        Transform bun = bln.GetChild(0);

        Transform ls = bt.GetChild(1);
        Transform le = ls.GetChild(0);
        Transform lw = le.GetChild(0);

        Transform rs = bt.GetChild(2);
        Transform re = rs.GetChild(0);
        Transform rw = re.GetChild(0);

        Transform lh = modelRoot.GetChild(1);
        Transform lk = lh.GetChild(0);
        Transform la = lk.GetChild(0);

        Transform rh = modelRoot.GetChild(2);
        Transform rk = rh.GetChild(0);
        Transform ra = rk.GetChild(0);

        bp.localPosition = new Vector3(0, height * .04f);
        bt.localPosition = new Vector3(0, height * .06f);
        bln.localPosition = new Vector3(0, height * .21f);
        bun.localPosition = new Vector3(0, height * .03f);

        ls.localPosition = new Vector3(-height * .11f, height * .188f);
        le.localPosition = new Vector3(0, -height * .188f);
        lw.localPosition = new Vector3(0, -height * .145f);

        rs.localPosition = new Vector3(height * .11f, height * .188f);
        re.localPosition = new Vector3(0, -height * .188f);
        rw.localPosition = new Vector3(0, -height * .145f);

        lh.localPosition = new Vector3(-.052f * height, 0);
        lk.localPosition = new Vector3(0, -height * .245f);
        la.localPosition = new Vector3(0, -height * .246f);

        rh.localPosition = new Vector3(.052f * height, 0);
        rk.localPosition = new Vector3(0, -height * .245f);
        ra.localPosition = new Vector3(0, -height * .246f);
    }


    //X is Z, Y is X, Z is Y
    private void MoveJoints()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(modelRoot.position, gizmoRadius);

        Gizmos.color = Color.yellow;

        foreach (Transform joint1 in modelRoot)
        {
            Gizmos.DrawSphere(joint1.position, gizmoRadius);
            Gizmos.DrawLine(modelRoot.position, joint1.position);

            foreach (Transform joint2 in joint1)
            {
                Gizmos.DrawSphere(joint2.position, gizmoRadius);
                Gizmos.DrawLine(joint1.position, joint2.position);

                foreach (Transform joint3 in joint2)
                {
                    Gizmos.DrawSphere(joint3.position, gizmoRadius);
                    Gizmos.DrawLine(joint2.position, joint3.position);

                    foreach (Transform joint4 in joint3)
                    {
                        Gizmos.DrawSphere(joint4.position, gizmoRadius);
                        Gizmos.DrawLine(joint3.position, joint4.position);

                        foreach (Transform joint5 in joint4)
                        {
                            Gizmos.DrawSphere(joint5.position, gizmoRadius);
                            Gizmos.DrawLine(joint4.position, joint5.position);
                        }
                    }
                }
            }
        }
    }
}
