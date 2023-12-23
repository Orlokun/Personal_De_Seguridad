using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FOV3D.Demo
{
    public class ScaleRandomize : MonoBehaviour
    {
        public List<GameObject> scaleObjects = new List<GameObject>();

        [Range(0.1f, 1f)] public float minStartScale = 0.1f;
        [Range(0.1f, 1f)] public float maxStartScale = 5f;

        [Range(1f, 5f)] public float minEndScale = 0.1f;
        [Range(1f, 5f)] public float maxEndScale = 5f;

        [Range(1f, 5f)] public float minDuration = 0.1f;
        [Range(1f, 5f)] public float maxDuration = 5f;
        private void Start()
        {
            RandomizeScale();
        }

        void RandomizeScale()
        {
            foreach (GameObject g in scaleObjects)
            {
                ScaleLerper scaleLerper = g.GetComponent<ScaleLerper>();
                if (scaleLerper != null)
                {
                    scaleLerper.startValue = Random.Range(minStartScale, maxStartScale);
                    scaleLerper.endValue = Random.Range(minEndScale, maxEndScale);
                    scaleLerper.duration = Random.Range(minDuration, maxDuration);
                }
            }
        }

    }
}
