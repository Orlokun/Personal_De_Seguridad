using System.Collections.Generic;
using GamePlayManagement.Players_NPC.Animations.Interfaces;

namespace GamePlayManagement.Players_NPC.Animations
{
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