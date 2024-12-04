using Main.Scenario;
using Main.UI;
using System;
using UnityEngine;

namespace Main.Events
{
    public static class MainEventsManager
    {
        public static Action<Transform> rightHandTargetTransformUpdate;
        public static Action<Transform> leftHandTargetTransformUpdate;
        public static Action<Transform> rightHandHintTransformUpdate;
        public static Action<Transform> leftHandHintTransformUpdate;
        public static Action<Transform> headTransformUpdate;
        public static Action<Transform> bodyTransformUpdate;

        public static Action<Vector3> currentHeadPosition;
        public static Action<Vector3> currentHeadEulerAngles;
        public static Action<Vector3> currentPlayerCameraPointPos;

        public static Action<Camera> onPlayerCamera;
        public static Action<JangadaMovement> onJangada;
        public static Action<IntroductionUI> onIntroUI;

        public static Action activateMimic;
        public static Action endOfPath;

        public static Action<float> pathProgression;
        public static Action pathStartMoving;
        public static Action pathReachingEnd;
        public static Action pathStopped;

        public static Action clipSaved;
        public static Action<bool> clipReady;
    }
}