using UnityEngine;

namespace FOV3D.Demo
{
    public class RotateAroundDemo : MonoBehaviour
    {
        public GameObject target;
        public float rotationSpeed = 20;
        // User Inputs
        public float amplitude = 0.5f;
        public float frequency = 1f;

        // Position Storage Variables
        Vector3 posOffset = new Vector3();
        Vector3 tempPos = new Vector3();
        void Start()
        {
            // Store the starting position & rotation of the object
            posOffset = transform.position;
        }
        void LateUpdate()
        {
            //tempPos = posOffset;
            // Spin the object around the target at 20 degrees/second.
            transform.RotateAround(target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            tempPos.y = posOffset.y + Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = new Vector3(transform.position.x, tempPos.y, transform.position.z);
        }
    }
}
