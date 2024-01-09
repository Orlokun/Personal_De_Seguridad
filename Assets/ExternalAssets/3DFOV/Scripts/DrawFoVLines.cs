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
    public class DrawFoVLines : MonoBehaviour, IDrawFoVLines
    {
        private Vector3[] mLineDirections; // Array of directions for each line
        [SerializeField] private LineRenderer _mGeneralSightLines;
        [SerializeField] private LineRenderer _mTargetLines; 
        [SerializeField] private Material generalSightMaterial;
        [SerializeField] private Material targetSightMaterial;

        void Awake()
        {
            ConfigureLines();
        }

        void ConfigureLines()
        {
            ConfigureGeneralViewLines();
            ConfigureTargetViewLines();
        }

        private void ConfigureGeneralViewLines()
        {
            _mGeneralSightLines.startWidth = 0.1f;
            _mGeneralSightLines.endWidth = 0.1f;
            _mGeneralSightLines.material = generalSightMaterial;
        }
        private void ConfigureTargetViewLines()
        {
            _mTargetLines.startWidth = 0.15f;
            _mTargetLines.endWidth = 0.15f;
            _mTargetLines.material = targetSightMaterial;
        }

        public void DrawTargetLineOfSight(Vector3 direction)
        {
            _mTargetLines.positionCount += 2;

            // Get the current number of lines
            var numberOfLines = _mTargetLines.positionCount / 2;

            // Set the positions
            var position = transform.position;
            _mTargetLines.SetPosition((numberOfLines - 1) * 2, position); // Start position of the new line
            _mTargetLines.SetPosition((numberOfLines - 1) * 2 + 1, direction + position); // End position of the new line
        }

        public void DrawDirectionLineOfSight(Vector3 direction)
        {
            // Increase the number of points
            _mGeneralSightLines.positionCount += 2;

            // Get the current number of lines
            var numberOfLines = _mGeneralSightLines.positionCount / 2;

            // Set the positions
            var position = transform.position;
            _mGeneralSightLines.SetPosition((numberOfLines - 1) * 2, position); // Start position of the new line
            _mGeneralSightLines.SetPosition((numberOfLines - 1) * 2 + 1, direction + position); // End position of the new line
        }

        public void ClearAllLines()
        {
            _mGeneralSightLines.positionCount = 0;
        }

        public void ClearTargetLines()
        {
            _mTargetLines.positionCount = 0;
        }
    }
}