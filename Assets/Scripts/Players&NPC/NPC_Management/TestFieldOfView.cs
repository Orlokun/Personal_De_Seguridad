using UnityEngine;
namespace Players_NPC.NPC_Management
{
    public class TestFieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0,360)]
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public LineRenderer lineRenderer;
        public int edgeResolveIterations;
        public float edgeDstThreshold;
        public int lineCount = 100;

        void Start()
        {
            lineRenderer.positionCount = lineCount * edgeResolveIterations;
        }

        void LateUpdate()
        {
            DrawFieldOfView();
        }

        void DrawFieldOfView()
        {
            float stepAngleSize = viewAngle / edgeResolveIterations;
            int lineIndex = 0;
            for (int j = 0; j < lineCount; j++)
            {
                float angleZ = j * 360f / lineCount;
                for (int i = 0; i <= edgeResolveIterations; i++)
                {
                    float angleY = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                    ViewCastInfo newViewCast = ViewCast(angleY, angleZ);
                    lineRenderer.SetPosition(lineIndex, newViewCast.point);
                    lineIndex++;
                }
            }
        }

        ViewCastInfo ViewCast(float globalAngleY, float globalAngleZ)
        {
            Vector3 dir = DirFromAngle(globalAngleY, globalAngleZ);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngleY);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngleY);
            }
        }

        public Vector3 DirFromAngle(float angleInDegreesY, float angleInDegreesZ)
        {
            return new Vector3(Mathf.Sin(angleInDegreesY * Mathf.Deg2Rad) * Mathf.Cos(angleInDegreesZ * Mathf.Deg2Rad), Mathf.Sin(angleInDegreesZ * Mathf.Deg2Rad), Mathf.Cos(angleInDegreesY * Mathf.Deg2Rad) * Mathf.Cos(angleInDegreesZ * Mathf.Deg2Rad));
        }

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }
    }
}