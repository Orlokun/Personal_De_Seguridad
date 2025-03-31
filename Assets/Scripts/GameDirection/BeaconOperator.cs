using UnityEngine;

namespace GameDirection
{
    public class BeaconOperator : MonoBehaviour
    {
        private float _mRotationSpeed = 3.5f;

        private float _mLightOsccilationRange = .15f;
    
        public GameObject[] _mBeaconLights = new GameObject[2];

        private float currentTimeStep;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _mBeaconLights[0].SetActive(false);
            _mBeaconLights[1].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, _mRotationSpeed);
            currentTimeStep += Time.deltaTime;
            if (currentTimeStep > _mLightOsccilationRange)
            {
                currentTimeStep = 0;
                ToggleLights();
            }
        }

        private void ToggleLights()
        {
            foreach (var beaconLight in _mBeaconLights)
            {
                beaconLight.SetActive(!beaconLight.activeSelf);
            }
        }
    }
}