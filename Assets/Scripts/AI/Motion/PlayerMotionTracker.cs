using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.AI.Motion
{
    public class PlayerMotionTracker : MonoBehaviour
    {
        [Header("Trackers")]
        [SerializeField] private Transform leftHandTracker;
        [SerializeField] private Transform leftElbowTracker;
        [SerializeField] private Transform rightHandTracker;
        [SerializeField] private Transform rightElbowTracker;
        [SerializeField] private Transform chestTracker;
        [SerializeField] private Transform headTracker;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}