using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario
{
    public class BezierPathRender : MonoBehaviour
    {
        [SerializeField] private Transform[] points;

        [Header("Drawing")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private bool hideOnPlay = false;
        [SerializeField] private Color bezierLineColor;
        [SerializeField] private Color pathColor;
        [SerializeField, Range(.01f, .99f)] private float pathDrawDensity = .9f;

        private void Awake()
        {
            if (hideOnPlay)
            {
                drawGizmos = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos)
            {
                return;
            }

            Gizmos.color = bezierLineColor;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == null || points[i].GetChild(0) == null)
                {
                    continue;
                }

                Vector3 bezierPos = points[i].GetChild(0).localPosition;

                Gizmos.DrawLine(points[i].position, points[i].position + bezierPos);
                Gizmos.DrawLine(points[i].position, points[i].position - bezierPos);
            }

            Gizmos.color = pathColor;

            for (int i = 0;i < points.Length - 1; i++)
            {
                float t = 0;

                if (points[i] == null || points[i].GetChild(0) == null || points[i + 1] == null || points[i + 1].GetChild(0) == null)
                {
                    continue;
                }

                while (t <= 1)
                {
                    Transform bezierTrasform1 = points[i].GetChild(0);
                    Transform bezierTrasform2 = points[i + 1].GetChild(0);

                    Vector3 bezier1 = Vector3.Lerp(points[i].position, bezierTrasform1.position, t);
                    Vector3 bezier2 = Vector3.Lerp(bezierTrasform1.position, points[i + 1].position - bezierTrasform2.localPosition, t);
                    Vector3 bezier3 = Vector3.Lerp(points[i + 1].position - bezierTrasform2.localPosition, points[i + 1].position, t);

                    Vector3 bezier4 = Vector3.Lerp(bezier1, bezier2, t);
                    Vector3 bezier5 = Vector3.Lerp(bezier2, bezier3, t);

                    Vector3 curPosition = Vector3.Lerp(bezier4, bezier5, t);
                    Gizmos.DrawWireSphere(curPosition, .25f);

                    t += 1 - pathDrawDensity;
                }
            }
        }
    }
}