using System.Collections;
using System.Collections.Generic;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;

namespace FOV3D.Demo
{
    public class LerpViewDemo : MonoBehaviour
    {
        public FieldOfView3D fov3D;
        public enum VarType
        {
            ViewRadius,
            ViewAngle,
            ViewResolution
        }
        [SerializeField] private VarType varType;
        private float targetValue; // --- The Thing you want to Move
        [SerializeField] private float startValue; //Start Position when t = 0.
        [SerializeField] private float endValue; //End Position when t = 1.
        private float t; //Value used to interpolate between startPos and endPos
        [SerializeField] private float duration; ///the bigger the value, the slower the object.
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
            if (fov3D != null)
            {
                if (varType == VarType.ViewRadius)
                    fov3D.viewRadius = targetValue;
                if (varType == VarType.ViewAngle)
                    fov3D.viewAngle = targetValue;
                if (varType == VarType.ViewResolution)
                    fov3D.viewResolution = ((int)targetValue);
            }
        }
    }
}
