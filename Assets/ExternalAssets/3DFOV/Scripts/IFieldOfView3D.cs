using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ExternalAssets._3DFOV.Scripts
{
    public interface IFieldOfView3D
    {
        public bool IsDrawActive { get; }
        public void ToggleInGameFoV(bool isActive);
        public bool HasTargetsInRange { get; }
        public List<GameObject> SeenTargetCharacters { get; }
        public void SetupCharacterFoV(int fovRange);
        public int ViewResolution { get; }
        public float ViewAngle { get; }
        public float ViewRadius { get; }
        public event FieldOfView3D.OnCharacterSeen OnCharacterSeenEvent;
        public event FieldOfView3D.OnCharacterLost OnCharacterLostEvent;
    }
}