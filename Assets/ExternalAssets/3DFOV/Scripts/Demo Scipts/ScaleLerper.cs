using System.Collections;
using UnityEngine;

namespace FOV3D.Demo
{
    public class ScaleLerper : MonoBehaviour
    {
        private float targetValue; // --- The Thing you want to Move
        public float startValue; //Start Position when t = 0.
        public float endValue; //End Position when t = 1.
        private float t; //Value used to interpolate between startPos and endPos
        public float duration; ///the bigger the value, the slower the object.
        [SerializeField] private bool oscillate;

        private void Start() ///Start is called once on startup. Used for initializing.
        {
            StartCoroutine(LerpCoroutine());
        }
        IEnumerator LerpCoroutine()
        {
            float time = 0;
            while (time < duration)
            {
                t = (time / duration);
                targetValue = Mathf.Lerp(startValue, endValue, t);
                UpdateView();
                time += Time.deltaTime;
                yield return null;
            }
            targetValue = endValue; //resolving values and position after completion;
            t = 1;
            if (oscillate) StartCoroutine(LerpBackCoroutine());
        }

        IEnumerator LerpBackCoroutine()
        {
            float time = 0;
            while (time < duration)
            {
                t = (time / duration);
                targetValue = Mathf.Lerp(endValue, startValue, t);
                UpdateView();
                time += Time.deltaTime;
                yield return null;
            }
            targetValue = startValue; //resolving values and position after completion;
            t = 1;
            StartCoroutine(LerpCoroutine());
        }

        void UpdateView()
        {
            this.gameObject.transform.localScale = new Vector3(targetValue, targetValue, targetValue);
        }
    }
}