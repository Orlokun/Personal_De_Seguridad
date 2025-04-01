using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts
{
    public interface IDrawFoVLines
    {
        public void DrawTargetLineOfSight(Vector3 direction);
        public void DrawDirectionLineOfSight(Vector3 direction);
        public void ClearAllLines();
        public void ClearTargetLines();

    }
}