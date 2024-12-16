using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Mimics
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        private MimicAnimationHandler mHandler;

        private void Awake()
        {
            transform.parent.TryGetComponent(out mHandler);
        }

        public void CentralizeBodyEvent(float duration)
        {
            mHandler.CentralizeBody(duration);
        }
    }
}
