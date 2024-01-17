using System;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    [Serializable]
    public class PreBuiltBlendshape
    {
        [SerializeField] public string name;
        [SerializeField] public float weight;
        public PreBuiltBlendshape(string name, float weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }
}
