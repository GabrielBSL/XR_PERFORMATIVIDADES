using System;
using UnityEngine;

namespace Main.Events
{
    public static class MainEventsManager
    {
        public static Action<Transform> rightHandTransformUpdate;
        public static Action<Transform> leftHandTransformUpdate;
        public static Action<Transform> headTransformUpdate;
        public static Action<Transform> bodyTransformUpdate;
    }
}