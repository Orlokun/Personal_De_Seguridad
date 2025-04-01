using System.Collections;
using UnityEngine;

namespace LeanTween.Framework
{
    public class LeanTester : MonoBehaviour {
        public float timeout = 15f;

#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5
        public void Start(){
            StartCoroutine( timeoutCheck() );
        }

        IEnumerator timeoutCheck(){
            float pauseEndTime = Time.realtimeSinceStartup + timeout;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            if(LeanTest.testsFinished==false){
                Debug.Log(LeanTest.formatB("Tests timed out!"));
                LeanTest.overview();
            }
        }
#endif
    }
}