using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.CharacterGenerator
{
    [System.Serializable]
    public class MinMaxIndex
    {
        public int Min;
        public int Max;
        public int GetRandom(int max)
        {
            var v = Random.Range(Min, Max);
            return Mathf.Clamp(v, -1, max);
        }
    }
}