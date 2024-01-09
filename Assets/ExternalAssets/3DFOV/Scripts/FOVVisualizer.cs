using System.Collections;
using System.Collections.Generic;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;


namespace FOV3D
{
    [ExecuteInEditMode]
    public class FOVVisualizer : MonoBehaviour
    {
        [HideInInspector] public FieldOfView3D fov3D;
        public bool wireframeActive = true;
        public bool viewAllRaycastLines = false;
        public bool viewSeenObjectLines = true;
        public bool viewAllSpherecasts = false;
        public bool viewSpherecasts = true;
        [HideInInspector] public bool p_viewSpherecasts;
        public bool gizmosActive = true;
        public enum GizmoType
        {
            Point,
            Ray,
            Cube,
            Sphere,
            Disc,
        }
        //[HideIf("gizmosActive", false)]
        public GizmoType gizmoType;
        //[HideIfEnumValue("gizmoType", HideIf.Equal, (int)GizmoType.Point)]
        //[ConditionalField(nameof(GizmoType), true, GizmoType.Point)]
        [Range(.01f, .1f)] public float pointSize = 0.025f;
        public Color pointColor = Color.white;
        public Color detectionColor = Color.green;
        public Color raycastColor = Color.white;
        public Color spherecastColor = Color.white;
        public Color wireframeColor = Color.white;
        private bool colorinit = false;

        private float viewRadius;
        private float viewAngle;
        private int viewResolution;
        private List<Vector3> m_directions = new List<Vector3>();
        private List<Vector3> m_point = new List<Vector3>();
        private List<Vector3> spherePoints = new List<Vector3>();
        private List<GameObject> seenObjects = new List<GameObject>();

#if UNITY_EDITOR
        private void Awake()
        {
            Validate();
        }
        private void OnValidate()
        {
            Validate();
        }

        private void Validate()
        {
            if (this.gameObject.TryGetComponent(out FieldOfView3D f))
            {
                fov3D = f;
                viewRadius = f.viewRadius;
                viewAngle = f.viewAngle;
                viewResolution = f.viewResolution;
                m_directions = f.mDirections;
                m_point = f.mPoint;
                seenObjects = f.seenObjects;

                spherePoints = f.spherePoints;
            }
            else
                Debug.LogError("FieldOfView3D Not Found on" + this.gameObject.ToString());
        }

        private void Update()
        {
            if (fov3D.detectionType == FieldOfView3D.DetectionType.Spherecast)
                p_viewSpherecasts = true;
            else
                p_viewSpherecasts = false;


            viewRadius = fov3D.viewRadius;
            viewAngle = fov3D.viewAngle;
            viewResolution = fov3D.viewResolution;
            m_directions = fov3D.mDirections;
            m_point = fov3D.mPoint;
            seenObjects = fov3D.seenObjects;

            spherePoints = fov3D.spherePoints;
        }

        private void Reset()
        {
            if (!colorinit)
            {
                pointColor.a = 1;
                wireframeColor.a = 1;
                detectionColor.a = 1;
                raycastColor.a = 1;
                spherecastColor.a = 1;
                colorinit = true;
            }
        }
        void OnDrawGizmos()
        {
            #region Math
            Vector3 rightDirection = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
            Vector3 leftDirection = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
            Vector3 upDirection = Quaternion.AngleAxis(-viewAngle, transform.right) * transform.forward;
            Vector3 downDirection = Quaternion.AngleAxis(viewAngle, transform.right) * transform.forward;
            Vector3 straightDirection = transform.forward;

            Ray r = new Ray(transform.position, straightDirection * viewRadius);
            Vector3 arcCenter = r.GetPoint(viewRadius);
            Vector3 circleCenter = (((transform.position + leftDirection * viewRadius) + (transform.position + rightDirection * viewRadius)) / 2);
            float isCircleCenter = Vector3.Distance(circleCenter, transform.position);

            float diameter = (Vector3.Distance((transform.position + leftDirection * viewRadius), (transform.position + rightDirection * viewRadius)));
            float radius = (diameter / 2);
            #endregion


            if (viewAllSpherecasts)
            {
                Gizmos.color = pointColor;
                if (fov3D.detectionType == FieldOfView3D.DetectionType.Spherecast)
                    foreach (Vector3 v in m_directions)
                        Gizmos.DrawWireSphere(v + transform.position, fov3D.sphereCastRadius);
            }

            if (viewSpherecasts)
            {
                Gizmos.color = spherecastColor;
                if (fov3D.detectionType == FieldOfView3D.DetectionType.Spherecast)
                {
                    for (int i = 0; i < spherePoints.Count; i++)
                        Gizmos.DrawWireSphere(spherePoints[i], fov3D.sphereCastRadius);
                }
            }

            if (gizmosActive)
            {
                #region FibSphere
                Gizmos.color = pointColor;

                int dir = m_directions.Count;
                if (dir == viewResolution)
                {
                    for (int i = 0; i < viewResolution; i++)
                    {
                        if (gizmoType == GizmoType.Sphere)
                            Gizmos.DrawSphere(m_directions[i] + transform.position, pointSize);
                        else if (gizmoType == GizmoType.Cube)
                            Gizmos.DrawCube(m_directions[i] + transform.position, new Vector3(pointSize, pointSize, pointSize));
                        else if (gizmoType == GizmoType.Ray)
                            Debug.DrawRay(m_directions[i] + transform.position, ((m_directions[i] + transform.position) - transform.position).normalized * -pointSize, pointColor);
                        else if (gizmoType == GizmoType.Point)
                            Debug.DrawRay(m_directions[i] + transform.position, ((m_directions[i] + transform.position) - transform.position).normalized * -0.01f, pointColor);
                        else if (gizmoType == GizmoType.Disc)
                            UnityEditor.Handles.DrawWireDisc(m_directions[i] + transform.position, ((m_directions[i] + transform.position) - transform.position).normalized, pointSize);
                    }
                }
                #endregion
            }

            #region Points
            #region Extents
            //Gizmos.color = Color.green;
            //Gizmos.DrawSphere(arcCenter, pointSize);
            //Gizmos.DrawSphere(rightDirection * viewRadius + transform.position, pointSize);
            //Gizmos.DrawSphere(leftDirection * viewRadius + transform.position, pointSize);
            //Gizmos.DrawSphere(upDirection * viewRadius + transform.position, pointSize);
            //Gizmos.DrawSphere(downDirection * viewRadius + transform.position, pointSize);
            #endregion
            #region Origin
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position, pointSize);
            #endregion
            #region Centerpoint
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(circleCenter, pointSize);
            #endregion
            #endregion

            #region Wireframe
            if (wireframeActive)
            {
                var oldColor = UnityEditor.Handles.color;
                var color = wireframeColor;
                UnityEditor.Handles.color = color;
                Gizmos.color = color;

                #region Line Of Sight
                Gizmos.DrawRay(transform.position, straightDirection * viewRadius);
                #endregion
                #region Arc Endings
                UnityEditor.Handles.DrawWireArc(transform.position, transform.up, rightDirection, -viewAngle * 2, viewRadius);
                UnityEditor.Handles.DrawWireArc(transform.position, transform.right, downDirection, -viewAngle * 2, viewRadius);
                #endregion
                #region Circle
                Vector3 center = (((transform.position + leftDirection * viewRadius) + (transform.position + rightDirection * viewRadius)) / 2);
                UnityEditor.Handles.DrawWireDisc(center, transform.forward, radius);
                #region CenterCircle
                if (viewAngle >= 90)
                {
                    UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, viewRadius);
                }
                #endregion
                #endregion
                #region Cross
                #region + Cross
                //Gizmos.DrawLine(transform.position + leftDirection * viewRadius, transform.position + rightDirection * viewRadius);
                //Gizmos.DrawLine(transform.position + downDirection * viewRadius, transform.position + upDirection * viewRadius);
                #endregion
                #region X Cross
                var d1 = Quaternion.AngleAxis(45, transform.forward) * transform.right;
                var d2 = Quaternion.AngleAxis(-45, transform.forward) * transform.right;
                Ray r1 = new Ray(center, d1 * radius);
                Ray r2 = new Ray(center, -d1 * radius);
                Ray r3 = new Ray(center, d2 * radius);
                Ray r4 = new Ray(center, -d2 * radius);
                Vector3 p1 = r1.GetPoint(radius);
                Vector3 p2 = r2.GetPoint(radius);
                Vector3 p3 = r3.GetPoint(radius);
                Vector3 p4 = r4.GetPoint(radius);
                //Gizmos.DrawRay(center, d1 * radius);
                //Gizmos.DrawRay(center, -d1 * radius);
                //Gizmos.DrawRay(center, d2 * radius);
                //Gizmos.DrawRay(center, -d2 * radius);
                #endregion
                #endregion
                #region Cone
                #region + Cone
                Gizmos.DrawRay(transform.position, rightDirection * viewRadius);
                Gizmos.DrawRay(transform.position, leftDirection * viewRadius);
                Gizmos.DrawRay(transform.position, upDirection * viewRadius);
                Gizmos.DrawRay(transform.position, downDirection * viewRadius);
                #endregion
                #region X Cone
                Gizmos.DrawLine(transform.position, p1);
                Gizmos.DrawLine(transform.position, p2);
                Gizmos.DrawLine(transform.position, p3);
                Gizmos.DrawLine(transform.position, p4);
                #endregion
                #endregion
            }
            #endregion
        }
        public void DrawRaycastLines(int i)
        {
            Debug.DrawLine(transform.position, m_directions[i] + transform.position, raycastColor);
        }
        public void DrawObjectLines()
        {
            if (seenObjects != null)
            {
                if (!p_viewSpherecasts)
                {
                    foreach (Vector3 v in m_point)
                        Debug.DrawLine(transform.position, v, detectionColor);
                }
                else
                {
                    if (fov3D.detectionType == FieldOfView3D.DetectionType.Spherecast)
                        foreach (Vector3 v in spherePoints)
                            Debug.DrawLine(transform.position, v, detectionColor);
                }
            }
        }
#endif
    }
}
