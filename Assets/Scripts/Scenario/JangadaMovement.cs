using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scenario
{
    public class JangadaMovement : MonoBehaviour
    {
        [System.Serializable]
        private struct Path
        {
            public Transform[] points;
        }

        [SerializeField] private List<Path> paths;
        [SerializeField] private float timeFromOnePointToAnother = 5;
        [SerializeField] private bool rotateAlongsidePath = true;
        [SerializeField] private InputAction startInput;

        [Header("Debug")]
        [SerializeField] private bool startMove;

        private bool _traveling = false;
        private int _pathIndex = 0;

        private void Awake()
        {
            startInput.performed += _ => StartPathTraveling();
        }

        private void OnEnable()
        {
            startInput.Enable();
        }
        private void OnDisable()
        {
            startInput.Disable();
        }

        private void Update()
        {
            if(startMove)
            {
                startMove = false;
                StartCoroutine(moveAlongPath());
            }
        }

        private void StartPathTraveling()
        {
            StartCoroutine(moveAlongPath());
        }

        private IEnumerator moveAlongPath()
        {
            if(_pathIndex >= paths.Count || _traveling)
            {
                yield break;
            }

            _traveling = true;
            int pathSize = paths[_pathIndex].points.Length;

            for(int i = 0; i < pathSize - 1; i++)
            {
                float t = 0;
                Vector3 lastPosition = paths[_pathIndex].points[i].position;

                while (t < 1)
                {
                    t = Mathf.Clamp01(t + Time.deltaTime / timeFromOnePointToAnother);

                    Transform bezierTrasform1 = paths[_pathIndex].points[i].GetChild(0);
                    Transform bezierTrasform2 = paths[_pathIndex].points[i + 1].GetChild(0);

                    Vector3 bezier1 = Vector3.Lerp(paths[_pathIndex].points[i].position, bezierTrasform1.position, t);
                    Vector3 bezier2 = Vector3.Lerp(bezierTrasform1.position, paths[_pathIndex].points[i + 1].position - bezierTrasform2.localPosition, t);
                    Vector3 bezier3 = Vector3.Lerp(paths[_pathIndex].points[i + 1].position - bezierTrasform2.localPosition, paths[_pathIndex].points[i + 1].position, t);

                    Vector3 bezier4 = Vector3.Lerp(bezier1, bezier2, t);
                    Vector3 bezier5 = Vector3.Lerp(bezier2, bezier3, t);

                    Vector3 curPosition = Vector3.Lerp(bezier4, bezier5, t);

                    transform.position = curPosition;

                    if (rotateAlongsidePath)
                    {
                        Vector3 pathCurDirection = (curPosition - lastPosition).normalized;
                        transform.rotation = Quaternion.LookRotation(pathCurDirection, transform.up);
                        lastPosition = curPosition;
                    }

                    yield return null;
                }
            }

            _traveling = false;
            _pathIndex++;
        }
    }
}