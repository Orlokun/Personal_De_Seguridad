using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    [System.Serializable]
    public class CharacterElementsPreset
    {
        public string name;
        public Mesh[] mesh;
        public string[] hideParts;
        public float yOffset = 0;
        public Material[] mats;
    }
}