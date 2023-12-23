using UnityEngine;

namespace FOV3D.Demo
{
    public class FloaterRandomize : MonoBehaviour
    {
        public float degMin;
        public float degMax;
        public float ampMin;
        public float ampMax;
        public float freqMin;
        public float freqMax;
        void OnEnable()
        {
            var allobjects = FindObjectsOfType(typeof(Floater));
            foreach (Floater f in allobjects)
            {
                f.degreesPerSecond = Random.Range(degMin, degMax);
                f.amplitude = Random.Range(ampMin, ampMax);
                f.frequency = Random.Range(freqMin, freqMax);
            }
        }
    }
}
