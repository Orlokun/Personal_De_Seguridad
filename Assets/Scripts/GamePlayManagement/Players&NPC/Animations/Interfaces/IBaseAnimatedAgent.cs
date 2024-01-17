using UnityEngine;
using Utils;

namespace GamePlayManagement.Players_NPC.Animations.Interfaces
{
    public interface IBaseAnimatedAgent : IInitializeWithArg1<Animator>
    {
        public void ChangeAnimationState(string newAnimState);
        public string GetCurrentAnim();
        public float GetCurrentAnimationLength();
        public bool IsAnimationAvailable(string animState);
    }
}