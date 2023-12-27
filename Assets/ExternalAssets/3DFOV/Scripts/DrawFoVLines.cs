using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts 
{
    public interface IDrawFoVLines
    {
        public void DrawDirectionLine(Vector3 direction);
        public void ClearAllLines();
    }
    public class DrawFoVLines : MonoBehaviour, IDrawFoVLines
    {
        private Vector3[] mLineDirections; // Array of directions for each line
        private LineRenderer _mLineRenderer;
        [SerializeField] private Material myMaterial;

        void Awake()
        {
            _mLineRenderer = gameObject.GetComponent<LineRenderer>();
            ConfigureLine();
        }

        void ConfigureLine()
        {
            _mLineRenderer.startWidth = 0.1f;
            _mLineRenderer.endWidth = 0.1f;
            _mLineRenderer.material = myMaterial;
        }

        public void DrawDirectionLine(Vector3 direction)
        {
            // Increase the number of points
            _mLineRenderer.positionCount += 2;

            // Get the current number of lines
            var numberOfLines = _mLineRenderer.positionCount / 2;

            // Set the positions
            var position = transform.position;
            _mLineRenderer.SetPosition((numberOfLines - 1) * 2, position); // Start position of the new line
            _mLineRenderer.SetPosition((numberOfLines - 1) * 2 + 1, direction + position); // End position of the new line
        }

        public void ClearAllLines()
        {
            _mLineRenderer.positionCount = 0;
        }
    }
}