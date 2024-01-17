using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    [Serializable]
    public class PreBuiltData
    {
        [SerializeField] public string GroupName;
        [SerializeField] public List<Mesh> meshes = new List<Mesh>();
        [SerializeField] public List<Material> materials = new List<Material>();
    }
}
