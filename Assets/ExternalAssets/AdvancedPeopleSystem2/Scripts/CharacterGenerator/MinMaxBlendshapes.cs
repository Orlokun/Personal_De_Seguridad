using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.CharacterGenerator
{
    [System.Serializable]
    public class MinMaxBlendshapes
    {
        [Range(-100, 100)]
        public float Min;
        [Range(-100, 100)]
        public float Max;
        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}