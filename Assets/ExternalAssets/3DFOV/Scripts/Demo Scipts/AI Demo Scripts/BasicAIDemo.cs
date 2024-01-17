using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts.Demo_Scipts.AI_Demo_Scripts
{
    public class BasicAIDemo : MonoBehaviour
    {
        public Transform target;
        public Transform myTransform;
        public float speed;

        public bool targetSeen = false;

        void Update()
        {
            if (targetSeen)
            {
                Debug.DrawLine(transform.position, target.position, Color.red);

                myTransform.LookAt(target);
                myTransform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        public void TargetSeenDemoMethod()
        {
            //Debug.Log("Target Located");
            targetSeen = true;
        }
        public void TargetLostDemoMethod()
        {
            //Debug.Log("Target Lost");
            targetSeen = false;
        }

        public void ExampleMethod()
        {
            ///Do Stuff
        }
    }
}
