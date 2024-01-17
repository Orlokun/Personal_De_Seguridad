
using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts.Demo_Scipts
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Range(0.1f, 50f)] public float spawnRadius = 10f;
        [Range(1, 25)] public int spawnNum = 5;

        [Range(0.1f, 5f)] public float minScale = 0.1f;
        [Range(0.1f, 5f)] public float maxScale = 5f;
        public FloaterRandomize randomFloat;

        public bool viewSpawnRadius = true;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < spawnNum; i++)
            {
                SpawnObstacle();
            }
            randomFloat.enabled = true;
        }



        void SpawnObstacle()
        {
            float scale = Random.Range(minScale, maxScale);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
            cube.transform.rotation = Random.rotation;
            cube.transform.localScale = new Vector3(scale, scale, scale);
            cube.AddComponent<Floater>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (viewSpawnRadius)
                Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
#endif
    }
}
