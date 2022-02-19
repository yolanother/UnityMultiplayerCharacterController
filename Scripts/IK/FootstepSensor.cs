using System;
using ReactorScripts.Common.MCC.Animations;
using UnityEngine;

namespace DoubTech.MCC.IK
{
    public class FootstepSensor : MonoBehaviour
    {
        [SerializeField] private HumanBodyBones bone;
        [SerializeField] private float footDownThreshold = .01f;
        [SerializeField] private float minTimeBetweenSteps = .1f;

        private AnimationEventListener animListener;
        private float lastY;
        private bool down;
        private Transform root;
        private double lastPlayTime;

        private float min;
        private float max;

        private void Awake()
        {
            animListener = GetComponentInParent<AnimationEventListener>();
            root = GetComponentInParent<Animator>().transform;
            min = float.MaxValue;
        }

        private void Update()
        {
            var stepPosition = transform.position.y - root.transform.position.y;

            min = Mathf.Min(min, stepPosition);
            max = Mathf.Max(max, stepPosition);

            Debug.Log(stepPosition + " / " + ((max - min) * .25f));
            if (stepPosition < min + (max - min) * footDownThreshold && Time.time - lastPlayTime > minTimeBetweenSteps)
            {
                PlaySound();
            }
        }

        private void PlaySound()
        {
            lastPlayTime = Time.time;
            switch (bone)
            {
                case HumanBodyBones.LeftFoot:
                    animListener.OnLeftFootDown();
                    break;
                case HumanBodyBones.RightFoot:
                    animListener.OnRightFootDown();
                    break;
            }
        }
    }
}
