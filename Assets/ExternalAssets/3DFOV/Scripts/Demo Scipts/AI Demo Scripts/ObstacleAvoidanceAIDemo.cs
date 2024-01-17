using System.Collections.Generic;
using UnityEngine;

namespace ExternalAssets._3DFOV.Scripts.Demo_Scipts.AI_Demo_Scripts
{
    public class ObstacleAvoidanceAIDemo : MonoBehaviour
    {
        ///This is a quick and dirty demo, to demonstrate usecases for FOV3D. It is not very optimal.

        public FieldOfView3D fov3d;
        public float movementSpeed = 5;
        public float lookSpeed = 5;

        [Range(1f, 2f)] public float collisionAvoidanceWeight = 1;
        [Range(0f, 20f)] public float targetingWeight = 1;
        [Range(0f, 5f)] public float fleeWeight = 1;

        public List<GameObject> targets = new List<GameObject>();
        public List<GameObject> predators = new List<GameObject>();
        private Color memColor;

        public float respawnRange;

        private float debugDis;

        public int score;
        private void Start()
        {
            memColor = GetComponentInChildren<MeshRenderer>().material.color;
        }
        private void Update()
        {
            //transform.position += FindBestDir() * Time.deltaTime * movementSpeed;
            transform.position += transform.forward * Time.deltaTime * movementSpeed;

            //transform.Translate(FindBestDir() * Time.deltaTime * movementSpeed);
            if (predators.Count > 0)
            {
                transform.position += transform.forward * Time.deltaTime * (movementSpeed + fleeWeight);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((FindBestDir() * collisionAvoidanceWeight) - (FleeDir() * fleeWeight)), (lookSpeed * fleeWeight) * Time.deltaTime);
            }

            else if ((targets.Count > 0) && (predators.Count == 0))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((FindBestDir() * collisionAvoidanceWeight) - (TargetDir() * targetingWeight)), lookSpeed * Time.deltaTime);

            }
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FindBestDir() * collisionAvoidanceWeight), lookSpeed * Time.deltaTime);
        }

        public void TargetSpotted(GameObject target)
        {
            if (predators.Count == 0)
            {
                if (fov3d.seenObjects.Contains(target))
                {
                    targets.Add(target);
                    GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
                }
            }
        }
        public void TargetLost(GameObject target)
        {
            if (!fov3d.seenObjects.Contains(target))
                targets.Remove(target);

            GetComponentInChildren<MeshRenderer>().material.color = memColor;

        }
        public void PredatorSpotted(GameObject target)
        {
            if (fov3d.seenObjects.Contains(target))
            {
                predators.Add(target);
                GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            }
        }
        public void PredatorLost(GameObject target)
        {
            if (!fov3d.seenObjects.Contains(target))
                predators.Remove(target);
            GetComponentInChildren<MeshRenderer>().material.color = memColor;
        }

        Vector3 TargetDir()
        {
            Vector3 bestDir = transform.forward;
            for (int i = 0; i < targets.Count; i++)
            {
                debugDis = Vector3.Distance(transform.position, targets[i].transform.position);

                if (Vector3.Distance(transform.position, targets[i].transform.position) <= 1.75f)
                {
                    score += 1;
                    if (targets[i].GetComponent<FieldOfView3D>() != null)
                    {
                        FieldOfView3D t_fov3d = targets[i].GetComponent<FieldOfView3D>();
                        t_fov3d.seenObjects.Clear();
                    }
                    if (targets[i].transform.parent != null)
                        targets[i].transform.parent.position = new Vector3(Random.Range(-respawnRange, respawnRange), Random.Range(-respawnRange, respawnRange), Random.Range(-respawnRange, respawnRange));
                    else
                        targets[i].transform.position = new Vector3(Random.Range(-respawnRange, respawnRange), Random.Range(-respawnRange, respawnRange), Random.Range(-respawnRange, respawnRange));
                }


                bestDir = transform.position - targets[i].transform.position;
                return bestDir;
            }
            return bestDir;
        }

        Vector3 FleeDir()
        {
            Vector3 bestDir = transform.forward;
            for (int i = 0; i < predators.Count; i++)
            {
                bestDir = transform.position + predators[i].transform.position;
                return bestDir;
            }
            return bestDir;
        }

        Vector3 FindBestDir()
        {
            Vector3 bestDir = transform.forward;
            float furthUnobDst = 0;

            for (int i = 0; i < fov3d.mDirections.Count; i++)
            {
                Vector3 dir = fov3d.mDirections[i];
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, fov3d.viewRadius, fov3d.layerMask))
                {
                    //float dist = Vector3.Distance(dir, transform.position);
                    float dist = Vector3.Distance(hit.point, transform.position);
                    if (dist > furthUnobDst)
                    {
                        bestDir = dir;
                        furthUnobDst = dist;
                    }
                }
                else
                    return dir;
            }
            return bestDir;
        }

    }
}
