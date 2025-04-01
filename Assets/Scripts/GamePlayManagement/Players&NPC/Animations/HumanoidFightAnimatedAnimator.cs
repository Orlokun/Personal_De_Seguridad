using System.Collections.Generic;
using GamePlayManagement.Players_NPC.Animations.Interfaces;

namespace GamePlayManagement.Players_NPC.Animations
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
}