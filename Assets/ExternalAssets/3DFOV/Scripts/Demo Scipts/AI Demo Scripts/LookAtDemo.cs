using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FOV3D.Demo
{
    public class LookAtDemo : MonoBehaviour
    {
        public float lookSpeed = 5;

        public Transform target;
        public Transform headRoot;
        private Vector3 dir;
        private Vector3 startRot;
        private bool targetSeen = false;

        public void TargetSeen()
        {
            targetSeen = true;
        }
        public void TargetLost()
        {
            targetSeen = false;
        }
        private void Start()
        {
            startRot = (target.position - transform.position).normalized;
        }
        private void Update()
        {
            dir = (target.position - transform.position).normalized;
        }

        void LateUpdate()
        {
            if (targetSeen)
            {
                if (dir.z > 0)
                {
                    Debug.DrawLine(transform.position, target.position, Color.red);
                    var rot = Quaternion.LookRotation(dir);
                    headRoot.rotation = Quaternion.Slerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
                }
                else
                {
                    headRoot.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startRot.normalized), lookSpeed * Time.deltaTime);
                }
            }
            else
            {
                headRoot.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startRot.normalized), lookSpeed * Time.deltaTime);
            }
        }
    }
}
