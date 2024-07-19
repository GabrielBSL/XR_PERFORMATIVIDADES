using Main.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Main.AI
{
    public class ForestAnimationDecisionMaker : MonoBehaviour
    {
        [SerializeField] private Transform headOriginReferenceTransform;

        private static RandomForest forest;

        private string jsonFilePath = "/movementData.json";
        private int _loadAmount = 0;

        private static float[] _playerMoveData = new float[180];
        private static AnimationClip[] _clips;

        private Transform _playerLeftHand;
        private Transform _playerRightHand;
        private Transform _playerHead;

        private float _playerDataTimer = 0;

        private void OnEnable()
        {
            MainEventsManager.rightHandTargetTransformUpdate += ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate += ReceiveLeftArmTarget;
            MainEventsManager.headTransformUpdate += ReceiveHeadReference;
        }

        private void OnDisable()
        {
            MainEventsManager.rightHandTargetTransformUpdate -= ReceiveRightArmTarget;
            MainEventsManager.leftHandTargetTransformUpdate -= ReceiveLeftArmTarget;
            MainEventsManager.headTransformUpdate -= ReceiveHeadReference;
        }

        private void ReceiveHeadReference(Transform playerHeadTransform)
        {
            _playerHead = playerHeadTransform;
        }

        private void ReceiveLeftArmTarget(Transform playerLeftHandTransform)
        {
            _playerLeftHand = playerLeftHandTransform;
        }

        private void ReceiveRightArmTarget(Transform playerRightHandTransform)
        {
            _playerRightHand = playerRightHandTransform;
        }

        private void Awake()
        {
            List<AnimationClip> clipsList = Resources.LoadAll<AnimationClip>("Animations/Muse").ToList();
            clipsList.RemoveAt(0);
            _clips = clipsList.ToArray();
        }

        void Start()
        {
            string path = Application.persistentDataPath + jsonFilePath;

            if (!File.Exists(path))
            {
                List<DataPoint> defaultData = new List<DataPoint>();
                StartCoroutine(CreateMovementDataSet(defaultData, path));
            }

            else
            {
                List<DataPoint> data = DataLoader.LoadData(path);

                forest = new RandomForest(100);
                forest.Train(data);
            }
        }

        private void Update()
        {
            _playerDataTimer += Time.deltaTime;

            if (_playerDataTimer < 1)
            {
                return;
            }

            _loadAmount = Mathf.Min(10, _loadAmount + 1);
            _playerDataTimer = 0;
            RearrangePlayerData();
        }

        private void RearrangePlayerData()
        {
            for (int i = 18; i < 180; i += 18)
            {
                for (int j = 0; j < 18; j++)
                {
                    _playerMoveData[i - (18 - j)] = _playerMoveData[i + j];
                }

                _playerMoveData[i + 0] = _playerLeftHand.localPosition.x;
                _playerMoveData[i + 1] = _playerLeftHand.localPosition.y;
                _playerMoveData[i + 2] = _playerLeftHand.localPosition.z;
                _playerMoveData[i + 3] = _playerLeftHand.localEulerAngles.x;
                _playerMoveData[i + 4] = _playerLeftHand.localEulerAngles.y;
                _playerMoveData[i + 5] = _playerLeftHand.localEulerAngles.z;

                _playerMoveData[i + 6] = _playerRightHand.localPosition.x;
                _playerMoveData[i + 7] = _playerRightHand.localPosition.y;
                _playerMoveData[i + 8] = _playerRightHand.localPosition.z;
                _playerMoveData[i + 9] = _playerRightHand.localEulerAngles.x;
                _playerMoveData[i + 10] = _playerRightHand.localEulerAngles.y;
                _playerMoveData[i + 11] = _playerRightHand.localEulerAngles.z;

                _playerMoveData[i + 12] = _playerHead.position.x - headOriginReferenceTransform.position.x;
                _playerMoveData[i + 13] = _playerHead.position.y - headOriginReferenceTransform.position.y;
                _playerMoveData[i + 14] = _playerHead.position.z - headOriginReferenceTransform.position.z;
                _playerMoveData[i + 15] = _playerHead.eulerAngles.x;
                _playerMoveData[i + 16] = _playerHead.eulerAngles.y;
                _playerMoveData[i + 17] = _playerHead.eulerAngles.z;
            }
        }

        public int GetAnimationClip()
        {
            if(_loadAmount >= 10)
            {
                return -1;
            }

            DataPoint dataToPredict = new DataPoint() { Attributes = _playerMoveData.ToArray() };
            ForestResult forestResult = forest.Predict(dataToPredict);

            return forestResult.results[0].Item1;
        }

        private IEnumerator CreateMovementDataSet(List<DataPoint> defaultData, string path)
        {
            //leftpos + leftangle + rightpos + rightangle + headpos + headangle
            //(-1.5<x<0, 2<y<4, 0.25<z<1, 0<x<90, 270<y<360 270<z<360

            Vector2 xLeftPosLimits = new Vector2(-1.5f, 0);
            Vector2 xRightPosLimits = new Vector2(0, 1.5f);
            Vector2 yPosLimits = new Vector2(2, 4);
            Vector2 zPosLimits = new Vector2(.25f, 1);
            Vector2 xAngleLimits = new Vector2(0, 90);
            Vector2 yAngleLimits = new Vector2(270, 360);
            Vector2 zAngleLimits = new Vector2(270, 360);
            Vector2 xzHeadPosLimits = new Vector2(-2, 2);
            Vector2 yHeadPosLimits = new Vector2(1f, 2f);
            Vector2 zHeadAngleLimit = new Vector2(-90, 25);

            Vector2 randomPosVariation = new Vector2 (0, .75f);
            Vector2 randomAngleVariation = new Vector2 (0, 45);

            for (int i = 0; i < 100; i++)
            {
                int randClass = Random.Range(0, _clips.Length);
                List<float> accumulatedValues = new List<float>();

                for (int j = 0; j < 10; j++)
                {
                    Vector3 currentLeftHandPos = new Vector3(Random.Range(xLeftPosLimits.x, xLeftPosLimits.y), Random.Range(yPosLimits.x, yPosLimits.y), Random.Range(zPosLimits.x, zPosLimits.y));
                    Vector3 currentLeftHandAgle = new Vector3(Random.Range(xAngleLimits.x, xAngleLimits.y), Random.Range(yAngleLimits.x, yAngleLimits.y), Random.Range(zAngleLimits.x, zAngleLimits.y));
                    Vector3 currentRightHandPos = new Vector3(Random.Range(xRightPosLimits.x, xRightPosLimits.y), Random.Range(yPosLimits.x, yPosLimits.y), Random.Range(zPosLimits.x, zPosLimits.y));
                    Vector3 currentRightHandAngle = new Vector3(Random.Range(xAngleLimits.x, xAngleLimits.y), Random.Range(yAngleLimits.x, yAngleLimits.y), Random.Range(zAngleLimits.x, zAngleLimits.y));
                    Vector3 currentHeadPos = new Vector3(Random.Range(xzHeadPosLimits.x, xzHeadPosLimits.y), Random.Range(yHeadPosLimits.x, yHeadPosLimits.y), Random.Range(xzHeadPosLimits.x, xzHeadPosLimits.y));
                    Vector3 currentHeadAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(zHeadAngleLimit.x, zHeadAngleLimit.y));

                    Insert(accumulatedValues, currentLeftHandPos);
                    Insert(accumulatedValues, currentLeftHandAgle);
                    Insert(accumulatedValues, currentRightHandPos);
                    Insert(accumulatedValues, currentRightHandAngle);
                    Insert(accumulatedValues, currentHeadPos);
                    Insert(accumulatedValues, currentHeadAngle);
                }

                DataPoint dataPoint = new DataPoint()
                {
                    Attributes = accumulatedValues.ToArray(),
                    Class = randClass
                };

                defaultData.Add(dataPoint);
                Debug.Log("entry added " + dataPoint.Attributes.Length);
            }

            Debug.Log("Data created");
            yield return null;

            DataSet defaultDataSet = new DataSet() { data = defaultData.ToArray() };
            string defaultJson = JsonUtility.ToJson(defaultDataSet);
            File.WriteAllText(path, defaultJson);

            forest = new RandomForest(100);
            forest.Train(defaultData);
        }

        public static void PrintValue(string value)
        {
            Debug.Log(value);
        }

        private void Insert(List<float> data, Vector3 values)
        {
            data.Add(values.x);
            data.Add(values.y);
            data.Add(values.z);
        }

        public static ForestResult GetCurrentResult()
        {
            ForestResult forestPredict = forest.Predict(new DataPoint() { Attributes = _playerMoveData });
            forestPredict.clips = new AnimationClip[forestPredict.results.Count];

            for (int i = 0; i < forestPredict.clips.Length; i++)
            {
                forestPredict.clips[i] = _clips[forestPredict.results[i].Item1];
            }

            return forestPredict;
        }
    }
}
