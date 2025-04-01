using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    [System.Serializable]
    public class CharacterAnimationPreset
    {
        public string name;

        public List<BlendshapeEmotionValue> blendshapes = new List<BlendshapeEmotionValue>();

        public bool UseGlobalBlendCurve = true;
        public AnimationCurve GlobalBlendAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
        [HideInInspector]
        public float AnimationPlayDuration = 1.0f;
        [HideInInspector]
        public float weightPower = 1.0f;
        [Header("May decrease performance")]
        public bool applyToAllCharacterMeshes;
    }
}