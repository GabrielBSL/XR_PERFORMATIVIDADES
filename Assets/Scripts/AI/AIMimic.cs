using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.AI
{
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
                Debug.Log(i++);
            }
        }

        private IEnumerator ForestCallCoroutine()
        {
            yield return new WaitForSeconds(firstCallDelay);

            while (true)
            {
                var predict = ForestDecisionMaker.GetCurrentResult();
                animator.transform.position = resetPosition.position;
                Debug.Log("play: " + predict.results[0].Item1);
                animator.Play(predict.results[0].Item1.ToString(), -1, 0f);

                yield return new WaitForSeconds(durationForNextAnimation);
            }
        }
    }
}