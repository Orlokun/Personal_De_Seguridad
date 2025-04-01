using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.CharacterGenerator
{
    [System.Serializable]
    public class MinMaxColor
    {
        public List<Color> minColors = new List<Color>();
        public List<Color> maxColors = new List<Color>();

        public Color GetRandom()
        {
            var index = Random.Range(0, minColors.Count);

            return Color.Lerp(minColors[index], maxColors[index], Random.Range(0f, 1f));
        }
    }
}