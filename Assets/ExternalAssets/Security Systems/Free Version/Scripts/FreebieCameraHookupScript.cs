using UnityEngine;

namespace ExternalAssets.Security_Systems.Free_Version.Scripts
{
    public class FreebieCameraHookupScript : MonoBehaviour
    {
        private Camera securityCamera;

        public Camera SecurityCamera
        {
            get 
            { 
                if(securityCamera == null)
                {
                    GetCameraReference();
                }

                return securityCamera; 
            }
        }

        private void Awake()
        {
            GetCameraReference();
        }

        private void GetCameraReference()
        {
            securityCamera = GetComponent<Camera>();
        }
    }
}
