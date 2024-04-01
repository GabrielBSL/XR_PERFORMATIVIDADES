using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scenario
{
    public class BezierPath : MonoBehaviour
    {
        [System.Serializable]
        private struct BezierPoint
        {
            public Transform pathPoint;
            public Transform bezierPoint;
        }

        [SerializeField] private BezierPoint[] points;

        [Header("Drawing")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color bezierLineColor;
        [SerializeField] private Color pathColor;
        [SerializeField, Range(.01f, .99f)] private float pathDrawDensity = .9f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
                if (points[i].pathPoint == null || points[i].bezierPoint == null)
                {
                    continue;
                }

                Gizmos.DrawLine(points[i].pathPoint.position, points[i].pathPoint.position + points[i].bezierPoint.localPosition);
                Gizmos.DrawLine(points[i].pathPoint.position, points[i].pathPoint.position - points[i].bezierPoint.localPosition);
            }

            Gizmos.color = pathColor;

            for (int i = 0;i < points.Length - 1; i++)
            {
                float t = 0;

                if (points[i].pathPoint == null || points[i].bezierPoint == null || points[i + 1].pathPoint == null || points[i + 1].bezierPoint == null)
                {
                    continue;
                }

                while (t <= 1)
                {
                    Vector3 bezier1 = Vector3.Lerp(points[i].pathPoint.position, points[i].bezierPoint.position, t);
                    Vector3 bezier2 = Vector3.Lerp(points[i].bezierPoint.position, points[i + 1].pathPoint.position - points[i + 1].bezierPoint.localPosition, t);
                    Vector3 bezier3 = Vector3.Lerp(points[i + 1].pathPoint.position - points[i + 1].bezierPoint.localPosition, points[i + 1].pathPoint.position, t);

                    Vector3 bezier4 = Vector3.Lerp(bezier1, bezier2, t);
                    Vector3 bezier5 = Vector3.Lerp(bezier2, bezier3, t);

                    Vector3 curPosition = Vector3.Lerp(bezier4, bezier5, t);
                    Gizmos.DrawWireSphere(curPosition, .1f);

                    t += 1 - pathDrawDensity;
                }
            }
        }
    }
}