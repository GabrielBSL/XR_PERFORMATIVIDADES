using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
//using UnityEditor.Animations;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

namespace Main.AI
{
    /*
    public class AIMimic : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform resetPosition;
        private float firstCallDelay = 10;
        private float durationForNextAnimation = 5;

        private void Start()
        {
            StartCoroutine(ForestCallCoroutine());
            StartCoroutine(Count());
        }

        private IEnumerator Count()
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(1);
                //Debug.Log(i++);
            }
        }

        private IEnumerator ForestCallCoroutine()
        {
            yield return new WaitForSeconds(firstCallDelay);

            while (true)
            {
                ForestResult predict = ForestAnimationDecisionMaker.GetCurrentResult();
                animator.transform.position = resetPosition.position;
                Debug.Log("play: " + predict.results[0].Item1);

                AnimatorController animatorOverride = animator.runtimeAnimatorController as AnimatorController;

                AnimatorStateMachine stateMachine = animatorOverride.layers[0].stateMachine;
                foreach (ChildAnimatorState state in stateMachine.states)
                {
                    if (state.state.name == "defaultState")
                    {
                        state.state.motion = predict.clips[0];
                        Debug.Log("Animation clip changed successfully.");
                        break;
                    }
                }

                animator.Play("defaultState", -1, 0f);

                yield return new WaitForSeconds(durationForNextAnimation);
            }
        }
    }
    */
}