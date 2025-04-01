using System.Collections.Generic;
using ExternalAssets.AdvancedPeopleSystem2.Scripts.CharacterGenerator;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCharacterSettings", menuName = "Advanced People Pack/Settings", order = 1)]
    public class CharacterSettings : ScriptableObject
    {
        public GameObject OriginalMesh;
        public Material bodyMaterial;
        [Space(20)]
        public List<CharacterAnimationPreset> characterAnimationPresets = new List<CharacterAnimationPreset>();
        [Space(20)]
        public List<CharacterBlendshapeData> characterBlendshapeDatas = new List<CharacterBlendshapeData>();
        [Space(20)]
        public List<CharacterElementsPreset> hairPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> beardPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> hatsPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> accessoryPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> shirtsPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> pantsPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> shoesPresets = new List<CharacterElementsPreset>();
        public List<CharacterElementsPreset> item1Presets = new List<CharacterElementsPreset>();
        [Space(20)]
        public List<CharacterSettingsSelector> settingsSelectors = new List<CharacterSettingsSelector>();
        [Space(20)]
        public RuntimeAnimatorController Animator;
        public Avatar Avatar;
        [Space(20)]
        public CharacterGeneratorSettings generator;

        [Space(20)]
        public CharacterSelectedElements DefaultSelectedElements = new CharacterSelectedElements();

        [Space(20)]
        public bool DisableBlendshapeModifier = false;
        
    }
}