using System.Collections.Generic;
using ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    public class CurrentBlendshapeAnimation
    {
        public CharacterAnimationPreset preset;
        public List<BlendshapeEmotionValue> blendShapesTemp = new List<BlendshapeEmotionValue>();
        public float timer = 0;
    }
}