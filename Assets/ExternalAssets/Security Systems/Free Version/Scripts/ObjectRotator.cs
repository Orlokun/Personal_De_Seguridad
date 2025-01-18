using UnityEngine;

namespace ExternalAssets.Security_Systems.Free_Version.Scripts
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationPerSecond;

        private void Update()
        {
            transform.Rotate(rotationPerSecond * Time.deltaTime);
        }
    }
}
