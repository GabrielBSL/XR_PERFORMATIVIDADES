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

        public static Action<Vector3> defaultHeadPosition;

        public static Action activateMimic;
    }
}