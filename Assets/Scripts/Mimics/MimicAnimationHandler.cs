using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Mimics
{
    public class MimicAnimationHandler : MonoBehaviour
    {
        [SerializeField] private float animationStartDelay = .05f;
        [SerializeField] private Animator animator;

        private void Start()
        {
            StartCoroutine(DelayStartCoroutine());
        }

        IEnumerator DelayStartCoroutine()
        {
            yield return new WaitForSeconds(animationStartDelay);
            animator.Play("debug_entry", -1, normalizedTime: 0.0f);
        }
    }
}
