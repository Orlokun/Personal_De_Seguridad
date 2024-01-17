using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts.Demo_Scipts
{
    public class Floater : MonoBehaviour
    {
        public enum SinCos
        {
            SinPlus,
            SinMin,
            CosPlus,
            CosMin
        }
        public SinCos sinCos;

        // User Inputs
        public float degreesPerSecond = 15.0f;
        public float amplitude = 0.5f;
        public float frequency = 1f;

        // Position Storage Variables
        Vector3 posOffset = new Vector3();
        Vector3 tempPos = new Vector3();

        // Use this for initialization
        void Start()
        {
            // Store the starting position & rotation of the object
            posOffset = transform.position;
        }
        void Update()
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            if (sinCos == SinCos.SinPlus)
                tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (sinCos == SinCos.SinMin)
                tempPos.y -= Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (sinCos == SinCos.CosPlus)
                tempPos.y += Mathf.Cos(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (sinCos == SinCos.CosMin)
                tempPos.y -= Mathf.Cos(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }
}
