using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Main.Utils
{
    [ExecuteInEditMode]
    public class MimicAutoSetup : MonoBehaviour
    {
        [Header("Execute")]
        [SerializeField] private bool setup;

        private void Update()
        {
            if (setup && !Application.isPlaying)
            {
                setup = false;
                SetUpMimic();
            }
        }

        private void SetUpMimic()
        {
            Transform armature = transform.GetChild(0).GetChild(0);

            if(armature.childCount == 0)
            {
                armature = transform.GetChild(0).GetChild(1);
            }

            armature.localPosition = Vector3.zero;
            armature.localEulerAngles = Vector3.zero;
            armature.localScale = Vector3.one;

            Transform hips = armature.GetChild(0);
            Transform vkRig = transform.GetChild(1);

            hips.localPosition = new Vector3(0, hips.localPosition.y, hips.localPosition.z);

            SetUpLeg(hips.GetChild(0).GetChild(0).GetChild(0), vkRig.GetChild(3)); //left leg
            SetUpLeg(hips.GetChild(1).GetChild(0).GetChild(0), vkRig.GetChild(2)); //right leg

            SetUpHand(hips.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0), vkRig.GetChild(1)); // Left hand
            SetUpHand(hips.GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0), vkRig.GetChild(0)); // right hand

            SetUpHead(hips.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0), vkRig.GetChild(4)); //head

            TryGetComponent(out RigBuilder rigBuilder);
            rigBuilder.Build();
        }

        private void SetUpLeg(Transform legModel, Transform IKLeg)
        {
            IKLeg.GetChild(0).position = legModel.position;
            IKLeg.GetChild(0).rotation = legModel.rotation;

            IKLeg.TryGetComponent(out TwoBoneIKConstraint twoBoneIK);
            twoBoneIK.data.tip = legModel;
            twoBoneIK.data.mid = legModel.parent;
            twoBoneIK.data.root = legModel.parent.parent;
        }
        private void SetUpHand(Transform handModel, Transform IKHand)
        {
            IKHand.GetChild(0).position = handModel.position;
            IKHand.GetChild(0).rotation = handModel.rotation;

            IKHand.GetChild(2).position = handModel.position;

            IKHand.TryGetComponent(out TwoBoneIKConstraint twoBoneIK);
            twoBoneIK.data.tip = handModel;
            twoBoneIK.data.mid = handModel.parent;
            twoBoneIK.data.root = handModel.parent.parent;
        }
        private void SetUpHead(Transform headModel, Transform IKHead)
        {
            IKHead.GetChild(0).position = headModel.position;
            IKHead.GetChild(0).rotation = headModel.rotation;

            IKHead.TryGetComponent(out MultiParentConstraint multiParentConstraint);
            multiParentConstraint.data.constrainedObject = headModel;
        }
    }
}