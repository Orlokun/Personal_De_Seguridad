using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    [System.Serializable]
    public class CharacterBlendshapeData
    {
        public string blendshapeName;
        public CharacterBlendShapeType type;
        public CharacterBlendShapeGroup group;
        [HideInInspector]
        public float value;

        public CharacterBlendshapeData(string name, CharacterBlendShapeType t, CharacterBlendShapeGroup g, float value = 0f)
        {
            this.blendshapeName = name;
            this.type = t;
            this.group = g;
            this.value = value;
        }
        public CharacterBlendshapeData() { }
    }
}