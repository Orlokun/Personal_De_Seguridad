using System.Collections.Generic;
using Players_NPC.Animations.Interfaces;

namespace Players_NPC.Animations
{
    public class HumanoidFightAnimatedAnimator : BaseAnimatedAgent, IHumanoidFightAnimatedAgent
    {
        public static List<string> MyFightAnimations => new List<string>()
        {
            "Sparring",
            "Jab",
            "Jab2",
            "StrongJab",
            "FloorKick",
            "MeleeWeapon",
            "MeleeWeaponFloor",
        };
        public override bool IsAnimationAvailable(string animState)
        {
            return MyFightAnimations.Contains(animState) ||  base.IsAnimationAvailable(animState);
        }
    }
    public class CustomerAnimationBehavior : BaseAnimatedAgent, IHumanoidFightAnimatedAgent
    {
        public static List<string> CustomerAnimAgentAnimations => new List<string>()
        {
            "Paying",
            "Crawling",
            ""
        };
    }
}