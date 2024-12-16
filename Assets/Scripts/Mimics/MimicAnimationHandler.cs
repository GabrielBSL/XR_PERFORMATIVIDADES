using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicAnimationHandler : MonoBehaviour
    {
        [SerializeField] private float animationStartDelay = .05f;
        [SerializeField] private Animator animator;

        [Header("Debug")]
        [SerializeField] private bool debugMessages = false;

        private int entranceAmountCounter = 0;
        private int entranceToTrigger = 3;

        private void Start()
        {
            StartCoroutine(DelayStartCoroutine());
        }

        IEnumerator DelayStartCoroutine()
        {
            yield return new WaitForSeconds(animationStartDelay);
            animator.Play("move 0", -1, normalizedTime: 0.0f);
        }

        public void CentralizeBody(float tweenDuration)
        {
            entranceAmountCounter++;

            if(entranceAmountCounter < entranceToTrigger )
            {
                return;
            }

            entranceAmountCounter = 0;
            StartCoroutine(CentralizeBodyCoroutine(tweenDuration));
        }

        private IEnumerator CentralizeBodyCoroutine(float tweenDuration)
        {
            float timePassed = 0;
            float progress;
            float resultMultiplier;

            Vector3 snapTransformPos = animator.transform.localPosition;

            while (timePassed < tweenDuration)
            {
                yield return null;
                timePassed += Time.deltaTime;
                progress = timePassed / tweenDuration;

                if (progress < 0.5f)
                {
                    resultMultiplier = 4 * progress * progress * progress;
                }
                else
                {
                    float p = 2 * progress - 2;
                    resultMultiplier = 0.5f * (p * p * p + 2);
                }

                animator.transform.localPosition = snapTransformPos * (1 - resultMultiplier);
            }
        }
    }
}
