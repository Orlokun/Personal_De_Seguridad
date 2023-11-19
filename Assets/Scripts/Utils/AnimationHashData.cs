using UnityEngine;
namespace Utils
{
    public static class AnimationHashData
    {
        public static readonly int BgFadeIn = Animator.StringToHash("ObjectFadeIn");
        public static readonly int BgFadeOut = Animator.StringToHash("ObjectFadeOut");
        public static readonly int TextFadeIn = Animator.StringToHash("TextFadeIn");
        public static readonly int TextFadeOut = Animator.StringToHash("TextFadeOut");        
    }
}