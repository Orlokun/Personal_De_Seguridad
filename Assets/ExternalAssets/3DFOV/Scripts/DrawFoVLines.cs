using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts 
{
    public interface IDrawLines
    {
        public void DrawDirectionLine(Vector3 direction);
        public void ClearAllLines();
    }
    public class DrawFoVLines : MonoBehaviour, IDrawLines
    {
        public Vector3[] m_lineDirections; // Array of directions for each line
        public Color raycastColor = Color.cyan; // Color of the lines
        private LineRenderer _mLineRenderer;
        [SerializeField] private Material myMaterial;
        void Start()
        {
            _mLineRenderer = gameObject.AddComponent<LineRenderer>();
            ConfigureLine();
        }

        void ConfigureLine()
        {
            _mLineRenderer.startWidth = 0.1f;
            _mLineRenderer.endWidth = 0.1f;
            // Create a new Material
            _mLineRenderer.material = myMaterial;
        }
        
        public void DrawDirectionLine(Vector3 direction)
        {
            // Increase the number of points
            _mLineRenderer.positionCount += 2;

            // Get the current number of lines
            int numberOfLines = _mLineRenderer.positionCount / 2;

            // Set the positions
            _mLineRenderer.SetPosition((numberOfLines - 1) * 2, transform.position); // Start position of the new line
            _mLineRenderer.SetPosition((numberOfLines - 1) * 2 + 1, direction + transform.position); // End position of the new line
        }

        public void ClearAllLines()
        {
            _mLineRenderer.positionCount = 0;
        }
    }
}