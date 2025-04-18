using System.Collections.Generic;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
using UnityEngine;
using Utils;

namespace GamePlayManagement.Players_NPC.Animations
{
    public class BaseAnimatedAgent : MonoBehaviour ,IBaseAnimatedAgent, IInitializeWithArg1<Animator>
    {
        private string _mCurrentAnimState;
        protected Animator Animator;
        protected const string Idle = "Idle";
        protected const string Walk = "Walk";
        protected const string Run = "Run";        
        public static List<string> BaseAnimAgentAnimations => new List<string>()
        {
            Idle,
            Walk,
            "Sit",
            "Chatting",
            Run,
            "Jumping",
            "Falling",
            "Sitting",
            "PanicRunning",
            "SearchAround",
            "DeathShot",
            "TargetRifleShoot"
        };
        
        #region Initialization
        protected bool MIsInitialized;
        public bool IsInitialized => MIsInitialized;
        public void Initialize(Animator injectionClass)
        {
            if (injectionClass == null)
            {
                return;
            }
        
            if (MIsInitialized || Animator != null)
            {
                return;
            }
            Animator = injectionClass;
            ChangeAnimationState("Idle");
            MIsInitialized = true;
        }
        #endregion

        public void ChangeAnimationState(string newAnimState)
        {
            if (!IsAnimationAvailable(newAnimState))
            {
                Debug.LogWarning($"Animation {newAnimState} is not valid");
                return;
            }
            _mCurrentAnimState = newAnimState;
            Animator.Play(_mCurrentAnimState);
        }

        public string GetCurrentAnim()
        {
            var currentAnim = Animator.GetCurrentAnimatorStateInfo(0).ToString();
            Debug.Log($"[BaseAnimatedAgent.{gameObject.name}.GetCurrentAnim] {currentAnim}");
            return currentAnim; 
        }

        public virtual bool IsAnimationAvailable(string animState)
        {
            return BaseAnimAgentAnimations.Contains(animState);
        }
        
        public float GetCurrentAnimationLength()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).length;
        }

    }
}