using System;
using DoubTech.MCC.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC
{
    public class CharacterAnimationEventManager : MonoBehaviour
    {
        [SerializeField] private AnimationClip[] deathAnimations;
        [SerializeField] private AnimationClip[] hitAnimations;

        [SerializeField] private UnityEvent onDeathAnimation = new UnityEvent();
        [SerializeField] private UnityEvent onHitAnimation = new UnityEvent();
        
        private IAnimationController animController;

        protected virtual void Awake()
        {
            animController = GetComponentInChildren<IAnimationController>();
        }

        public virtual void OnDied()
        {
            if (null == deathAnimations || deathAnimations.Length == 0) return;
            
            animController.PlayAction(deathAnimations.Random());
            onDeathAnimation.Invoke();
        }

        public virtual void OnHit()
        {
            if (null == hitAnimations || hitAnimations.Length == 0) return;
            
            animController.PlayAction(hitAnimations.Random());
            onHitAnimation.Invoke();
        }

        public void PlayAction(AnimationClip clip) => animController.PlayAction(clip);
        public void PlayLoop(AnimationClip clip) => animController.PlayLoopingAction(clip);
        public void PlayTrigger(string trigger) => animController.PlayTrigger(trigger);
    }
}