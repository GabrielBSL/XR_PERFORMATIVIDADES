using Main.Events;
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
            public float timeFromOnePointToAnother; 
            public float deltaToStartMove;
            public float minimalTimeToMove;
            public Transform[] points;
        }

        [SerializeField] private List<Path> paths;
        [SerializeField] private bool rotateAlongsidePath = true;
        [SerializeField] private InputAction startInput;

        [Header("End of path")]
        [SerializeField] private float endOfPathDelta;
        [SerializeField] private float endOfPathDuration;

        [Header("Debug")]
        [SerializeField] private bool startMove;

        private bool _traveling = false;

        private int _pathIndex = 0;
        private static float _totalDelta = 0;
        private float _timeToMoveTimer = 0;
        private PoseInfo _lastPose;

        private bool _toCalculate = false;

        public static float TotalDelta { get { return _totalDelta; } private set { _totalDelta = value; } }

        private void Start()
        {
            _lastPose = RewindController.GetLastPose();
            MainEventsManager.onJangada?.Invoke(this);
        }

        public void StartCalculation()
        {
            _toCalculate = true;
        }

        private void Update()
        {
            if (!_toCalculate)
            {
                return;
            }

            CalculatePlayerMovement();

            if(_pathIndex < paths.Count)
            {
                //Debug.Log($"Total Delta = {TotalDelta}");
                if(TotalDelta > paths[_pathIndex].deltaToStartMove && _timeToMoveTimer > paths[_pathIndex].minimalTimeToMove)
                {
                    StartCoroutine(moveAlongPath());
                }
            }
            else if(TotalDelta > endOfPathDelta && _timeToMoveTimer > endOfPathDuration)
            {
                StartCoroutine(moveAlongPath());
            }

            if(startMove)
            {
                startMove = false;
                StartCoroutine(moveAlongPath());
            }
            //GestureReferenceEvents.jangadaMoving?.Invoke(_traveling);
        }

        private void CalculatePlayerMovement()
        {
            if(_traveling)
            {
                return;
            }

            _timeToMoveTimer += Time.deltaTime;
            PoseInfo pose = RewindController.GetLastPose();
            Vector3 deltaVector = new Vector3(Mathf.Abs(pose.rightTargetLocalPos.x - _lastPose.rightTargetLocalPos.x),
                                           Mathf.Abs(pose.rightTargetLocalPos.y - _lastPose.rightTargetLocalPos.y),
                                           Mathf.Abs(pose.rightTargetLocalPos.z - _lastPose.rightTargetLocalPos.z));

            deltaVector += new Vector3(Mathf.Abs(pose.leftTargetLocalPos.x - _lastPose.leftTargetLocalPos.x),
                                       Mathf.Abs(pose.leftTargetLocalPos.y - _lastPose.leftTargetLocalPos.y),
                                       Mathf.Abs(pose.leftTargetLocalPos.z - _lastPose.leftTargetLocalPos.z));

            TotalDelta += Vector3.Distance(Vector3.zero, deltaVector);
            _lastPose = pose;
        }

        private IEnumerator moveAlongPath()
        {
            _timeToMoveTimer = 0;
            TotalDelta = 0;

            if (_pathIndex >= paths.Count)
            {
                MainEventsManager.endOfPath?.Invoke();
                yield break;
            }
            if (_traveling)
            {
                yield break;
            }

            MainEventsManager.pathStartMoving?.Invoke();
            _traveling = true;
            int pathSize = paths[_pathIndex].points.Length;

            for(int i = 0; i < pathSize - 1; i++)
            {
                if(i == pathSize - 2)
                {
                    MainEventsManager.pathReachingEnd?.Invoke();
                }

                float t = 0;
                Vector3 lastPosition = paths[_pathIndex].points[i].position;

                while (t < 1)
                {
                    t = Mathf.Clamp01(t + Time.deltaTime / paths[_pathIndex].timeFromOnePointToAnother);

                    Transform bezierTrasform1 = paths[_pathIndex].points[i].GetChild(0);
                    Transform bezierTrasform2 = paths[_pathIndex].points[i + 1].GetChild(0);

                    Vector3 bezier1 = Vector3.Lerp(paths[_pathIndex].points[i].position, bezierTrasform1.position, t);
                    Vector3 bezier2 = Vector3.Lerp(bezierTrasform1.position, paths[_pathIndex].points[i + 1].position - bezierTrasform2.localPosition, t);
                    Vector3 bezier3 = Vector3.Lerp(paths[_pathIndex].points[i + 1].position - bezierTrasform2.localPosition, paths[_pathIndex].points[i + 1].position, t);

                    Vector3 bezier4 = Vector3.Lerp(bezier1, bezier2, t);
                    Vector3 bezier5 = Vector3.Lerp(bezier2, bezier3, t);

                    Vector3 curPosition = Vector3.Lerp(bezier4, bezier5, t);

                    //GestureReferenceEvents.aldeiaBlend?.Invoke(Mathf.Clamp(_pathIndex + (i + t)/(pathSize - 1) - 4, 0, 3));
                
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

            _lastPose = RewindController.GetLastPose();
            MainEventsManager.pathStopped?.Invoke();
            _traveling = false;
            _pathIndex++;
        }
    }
}
