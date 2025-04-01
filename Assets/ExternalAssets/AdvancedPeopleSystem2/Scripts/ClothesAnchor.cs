using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    [System.Serializable]
    public class ClothesAnchor
    {
        public CharacterElementType partType;
        public List<SkinnedMeshRenderer> skinnedMesh;
        public ClothesAnchor()
        {
            skinnedMesh = new List<SkinnedMeshRenderer>();
        }
    }
}