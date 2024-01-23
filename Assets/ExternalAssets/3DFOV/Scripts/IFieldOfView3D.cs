using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts
{
    public interface IFieldOfView3D
    {
        public bool IsDrawActive { get; }
        public void ToggleInGameFoV(bool isActive);
        public bool HasTargetsInRange { get; }
        public List<GameObject> SeenTargetObjects { get; }
        public void SetupCharacterFoV(int fovRange);
        public int ViewResolution { get; }
        public float ViewAngle { get; }
        public float ViewRadius { get; }
    }
}