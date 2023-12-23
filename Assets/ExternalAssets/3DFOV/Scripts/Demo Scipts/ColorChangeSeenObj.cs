using System.Collections.Generic;
using UnityEngine;

namespace FOV3D.Demo
{
    public class ColorChangeSeenObj : MonoBehaviour
    {
        public FieldOfView3D fov3D;
        public Color viewColor;
        public List<GameObject> currentObj = new List<GameObject>();
        public List<GameObject> memObj = new List<GameObject>();
        public List<Color> pastColor = new List<Color>();
        private void Update()
        {
            currentObj = fov3D.seenObjects;

            foreach (GameObject obj in fov3D.seenObjects)
            {
                if (!memObj.Contains(obj))
                {
                    memObj.Add(obj);
                    Color pc = obj.GetComponent<Renderer>().material.color;
                    pastColor.Add(pc);
                    obj.GetComponent<Renderer>().material.color = viewColor;
                }
            }

            if (memObj.Count != currentObj.Count)
            {
                for (int i = 0; i < memObj.Count; i++)
                {
                    if (!currentObj.Contains(memObj[i]))
                    {
                        memObj[i].GetComponent<Renderer>().material.color = pastColor[i];
                        pastColor.RemoveAt(i);
                        memObj.RemoveAt(i);
                    }
                }
            }
        }
    }
}
