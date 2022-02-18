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
        
        protected IAnimationController animController;

        protected virtual AnimationClip[] DeathAnimations => deathAnimations;
        protected virtual AnimationClip[] HitAnimations => hitAnimations;

        protected virtual void Awake()
        {
            animController = GetComponentInChildren<IAnimationController>();
        }

        public virtual void OnDied()
        {
            if (null == DeathAnimations || DeathAnimations.Length == 0) return;
            var death = DeathAnimations.Random();
            Debug.Log($"Playing death animation: {death.name}");
            animController.OverrideWeights(0);
            animController.PlayLoopingAction(death);
            onDeathAnimation.Invoke();
        }

        public virtual void OnHit()
        {
            if (null == HitAnimations || HitAnimations.Length == 0) return;
            
            animController.PlayAction(HitAnimations.Random());
            onHitAnimation.Invoke();
        }

        public void PlayAction(AnimationClip clip) => animController.PlayAction(clip);
        public void PlayLoop(AnimationClip clip) => animController.PlayLoopingAction(clip);
        public void PlayTrigger(string trigger) => animController.PlayTrigger(trigger);
    }
}