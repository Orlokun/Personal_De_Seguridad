using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    [System.Serializable]
    public class CharacterPart
    {
        public string name;
        public List<SkinnedMeshRenderer> skinnedMesh;
        public CharacterPart()
        {
            skinnedMesh = new List<SkinnedMeshRenderer>();
        }
    }
}