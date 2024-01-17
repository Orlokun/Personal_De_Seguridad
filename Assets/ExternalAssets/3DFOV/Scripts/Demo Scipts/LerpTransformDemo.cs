using System.Collections;
using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts.Demo_Scipts
{
    public class LerpTransformDemo : MonoBehaviour
    {
        public Transform target; // --- The Thing you want to Move

        //-----------------------------------------------------------------------------------------------------------------------//
        //The Important Stuff --- Vector3 Lerp(Vector3 startPos, Vector3 endPos, float t);
        //-----------------------------------------------------------------------------------------------------------------------//
        public Vector3 startPos; //Start Position when t = 0.
        public Vector3 endPos; //End Position when t = 1.
        [Range(0f, 1f)] [SerializeField] private float t; //Value used to interpolate between startPos and endPos
                                                          //-----------------------------------------------------------------------------------------------------------------------//

        public float duration; ///the bigger the value, the slower the object.
        [SerializeField] private bool oscillate;
        private void Start() ///Start is called once on startup. Used for initializing.
        {
            StartCoroutine(LerpCoroutine());
        }

        IEnumerator LerpCoroutine()
        {
            float time = 0;
            startPos = target.position; //this makes the start position (startPos) wherever the object currently is.

            while (time < duration)
            {
                t = (time / duration);
                target.position = Vector3.Lerp(startPos, endPos, t);
                time += Time.deltaTime;
                yield return null;
            }
            target.position = endPos; //resolving values and position after completion;
            t = 1;
            if (oscillate) StartCoroutine(LerpBackCoroutine());
        }
        IEnumerator LerpBackCoroutine()
        {
            float time = 0;
            while (time < duration)
            {
                t = (time / duration);
                target.position = Vector3.Lerp(endPos, startPos, t);
                time += Time.deltaTime;
                yield return null;
            }
            target.position = startPos; //resolving values and position after completion;
            t = 1;
            StartCoroutine(LerpCoroutine());
        }
    }
}
