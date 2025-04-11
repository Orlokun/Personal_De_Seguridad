using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlayManagement.Players_NPC;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExternalAssets._3DFOV.Scripts
{
    [RequireComponent(typeof(DrawFoVLines))]
    public class FieldOfView3D : MonoBehaviour, IFieldOfView3D
    {
        #region BaseVariables
        private const float BaseViewRadius = 5;
        private const float BaseViewAngle = 30f;
        #endregion
        
        #region Variables
        [Range(0f, 50f)] private float _viewRadius;
        [Range(0f, 180f)] private float _viewAngle;
        [Range(1f, 2500f)] private const int MViewResolution = 100;

        public LayerMask layerMask;
        public enum DetectionType
        {
            Raycast,
            Linecast,
            Spherecast
        }
        public DetectionType detectionType;
        [Range(0.1f, 1f)] public float sphereCastRadius = 0.1f;

        #region Target Detection

        [FormerlySerializedAs("seenObjects")] public List<GameObject> seenCharacters = new List<GameObject>();
        public List<GameObject> targetObjects = new List<GameObject>();
        
        public int ViewResolution => MViewResolution;
        public float ViewRadius => _viewRadius;
        
        public delegate void OnCharacterSeen(GameObject target);
        public event OnCharacterSeen OnCharacterSeenEvent;
        public delegate void OnCharacterLost(GameObject target);
        public event OnCharacterLost OnCharacterLostEvent;
        public float ViewAngle => _viewAngle;
        [SerializeField] private bool detectionActive = true;
        #endregion

        private bool m_goldenRatio = true;
        [Range(0f, 2f)] private float _turnFraction;
        private float power = 1;
        private IDrawFoVLines _drawFoVLines;
        private bool _isDrawFoVActive;

        public void ToggleInGameFoV(bool isActive)
        {
            _isDrawFoVActive = isActive;
        }

        public bool HasTargetsInRange => seenCharacters.Count > 0;
        public List<GameObject> SeenTargetCharacters => seenCharacters;
        public void SetupCharacterFoV(int fovRange)
        {
            var fovRangeFloat = fovRange / 10f;
            
            _viewRadius = fovRange;
            _viewAngle = BaseViewAngle * fovRangeFloat;
        }
        public bool IsDrawActive => _isDrawFoVActive;
        
        [HideInInspector] public List<Vector3> mDirections = new List<Vector3>();
        [HideInInspector] public List<Vector3> mPoint = new List<Vector3>();
        [HideInInspector] public List<Vector3> spherePoints = new List<Vector3>();
        [HideInInspector] public List<int> hitIndexs = new List<int>();
        [HideInInspector] public List<int> missIndexs = new List<int>();
        [HideInInspector] public int hitIndexCount;
        [HideInInspector] public int missIndexCount;
        private float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        private bool _tempbool;
        #endregion
        private void Awake()
        {
            mDirections = new List<Vector3>(MViewResolution);
            seenCharacters = new List<GameObject>();
            mPoint = new List<Vector3>();
            _tempbool = false;
            _isDrawFoVActive = false;
            _drawFoVLines = gameObject.GetComponent<DrawFoVLines>();
        }
        
        private void Update()
        {
            if (mDirections.Count != MViewResolution)
            {
                StartCoroutine(ListSetup(mDirections, MViewResolution));
            }
            #region UpdateSetup
            if (m_goldenRatio) _turnFraction = goldenRatio;
            var angleIncrement = Mathf.PI * 2 * _turnFraction;
            var radians = _viewAngle * Mathf.Deg2Rad;
            var c = -1 * Mathf.Cos(radians) + 1;

            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x + 270, rot.y, 180);
            Quaternion myRotation = Quaternion.Euler(rot);
            _drawFoVLines.ClearAllLines();
            _drawFoVLines.ClearTargetLines();
            #endregion

            #region CalculateDirections
            for (var i = 0; i < MViewResolution; i++)
            {
                var t = (float)i / MViewResolution;
                var inclination = Mathf.Acos(1 - c * t);
                inclination = Mathf.Pow(inclination, power);
                var azimuth = angleIncrement * i;

                var x = Mathf.Sin(inclination) * Mathf.Cos(azimuth) * _viewRadius;
                var y = Mathf.Sin(inclination) * Mathf.Sin(azimuth) * _viewRadius;
                var z = Mathf.Cos(inclination) * _viewRadius;

                var endPoint = new Vector3(x, z, y);
                endPoint = myRotation * endPoint;

                mDirections[i] = endPoint;
                
                if (detectionActive)
                {
                    RaycastHit hit;
                    var dir = (mDirections[i]).normalized;

                    if (i == 0)
                    {
                        hitIndexCount = 0;
                        missIndexCount = 0;
                        hitIndexs.Clear();
                        missIndexs.Clear();
                        spherePoints.Clear();
                    }                    
                    switch (detectionType)
                    {
                        case DetectionType.Raycast:
                            if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, layerMask))
                                Detection(hit, i);
                            break;
                        case DetectionType.Linecast:
                            if (Physics.Linecast(transform.position, endPoint + transform.position, out hit, layerMask))
                                Detection(hit, i);
                            break;
                        case DetectionType.Spherecast:
                            if (Physics.SphereCast(transform.position, sphereCastRadius, dir, out hit, _viewRadius - sphereCastRadius, layerMask))
                            {                                
                                hitIndexs.Add(new int());
                                hitIndexs[hitIndexCount] = i;
                                spherePoints.Add(new Vector3());
                                spherePoints[hitIndexCount] = SphereCase(hit, i);
                                hitIndexCount++;
                                Detection(hit, i);
                            }
                            else
                            {
                                missIndexs.Add(new int());
                                missIndexs[missIndexCount] = i;
                                missIndexCount++;
                            }                                                          
                            break;
                    }
                }
                //if ((ValidateVisualizer()) && (_fovV.viewAllRaycastLines)) _fovV.DrawRaycastLines(i);
                ProcessInGameVisualization(mDirections[i]);
            }
            #endregion

            ProcessTargetsInSight();
        }

        private void ProcessTargetsInSight()
        {
            if (seenCharacters.Count > 0)
            {
                
            }
        }
        private void ProcessInGameVisualization(Vector3 direction)
        {
            if (_isDrawFoVActive)
            {
                _drawFoVLines.DrawDirectionLineOfSight(direction);
            }
            else
            {
                _drawFoVLines.ClearAllLines();
            }
        }


        private Vector3 SphereCase(RaycastHit hit, int i)
        {
            var midPoint = new Vector3();
            if (seenCharacters != null)
            {
                var myTransform = transform;
                var position = myTransform.position;
                Ray r = new Ray(position, mDirections[i]);
                var a = position;
                var b = hit.point;
                var c = r.GetPoint(_viewRadius - sphereCastRadius);

                var v1 = Vector3.Dot((c - a), (c - a));
                var v2 = Vector3.Dot((b - a), (c - a));
                var t = v2 / v1;

                midPoint = (a + t * (c - a));       
                return midPoint;
            }
            return midPoint;
        }
        
        private void Detection(RaycastHit hit, int i)
        {
            mDirections[i] = hit.point - transform.position;
            GameObject viewObj = hit.collider.gameObject;
            var isCharacter = viewObj.TryGetComponent<IBaseCharacterInScene>(out _);
            if (!isCharacter)
            {
                return;
            }
            if (!seenCharacters.Contains(viewObj))
            {
                seenCharacters.Add(viewObj);
                mPoint.Add(hit.point);
            }
            else
            {
                if (seenCharacters.Count() > 0)
                {
                    var index = seenCharacters.IndexOf(viewObj);
                    if (Vector3.Distance(transform.position, hit.point) < _viewRadius)
                        mPoint[index] = hit.point;
                }
            }
            if (targetObjects.Contains(viewObj))
            {
                if (!_tempbool)
                {
                    _tempbool = true;
                    StartCoroutine(OnTargetEventTrigger(viewObj));
                }
            }
            ProcessTargetInGameVisualization(mDirections[i]);
        }


        private void ProcessTargetInGameVisualization(Vector3 direction)
        {
            _drawFoVLines.DrawTargetLineOfSight(direction);
        }
        private void FixedUpdate()
        {
            if (seenCharacters.Count > 0)
            {
                for (int j = 0; j < seenCharacters.Count; j++)
                {
                    Collider myCollider = seenCharacters[j].GetComponent<Collider>();
                    if (!CheckPointInsideCone(mPoint[j], transform.position, transform.forward, _viewAngle, _viewRadius))
                    {
                        RemoveFromSight(j);
                        break;
                    }
                    else if (!CheckPointInsideCone(myCollider.bounds.max, transform.position, transform.forward, _viewAngle, _viewRadius))
                    {
                        if (myCollider.bounds.SqrDistance(mPoint[j]) > .01f)
                        {
                            RemoveFromSight(j);
                            break;
                        }
                    }

                    if (!CheckObstruction(seenCharacters[j], mPoint[j]))
                        RemoveFromSight(j);
                }
            }
        }
        private void RemoveFromSight(int j)
        {
            if (j >= 0 && j < seenCharacters.Count())
                seenCharacters.RemoveAt(j);
            if (j >= 0 && j < mPoint.Count())
                mPoint.RemoveAt(j);
            if (detectionType == DetectionType.Spherecast)
                if (j >= 0 && j < spherePoints.Count())
                    spherePoints.RemoveAt(j);
        }
        private IEnumerator ListSetup(List<Vector3> list, int viewResolution)
        {
            while (list.Count != viewResolution)
            {
                if (list.Count < viewResolution)
                    list.Add(new Vector3());
                if (list.Count > viewResolution)
                    list.RemoveAt(list.Count - 1);
            }
            yield return null;
        }
        bool CheckObstruction(GameObject og, Vector3 point)
        {
            var position = transform.position;
            var dir = (point - position);
            
            float dis = Vector3.Distance(position, point);
            RaycastHit hitCheck;
            if (Physics.Linecast(transform.position, point, out hitCheck, layerMask))
            {
                var g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            else if (Physics.Raycast(transform.position, dir, out hitCheck, dis, layerMask))
            {
                var g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            else if (Physics.SphereCast(transform.position, .01f, dir, out hitCheck, dis, layerMask))
            {
                var g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            return false;
        }
        bool CheckPointInsideCone(Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle, float maxDistance)
        {
            var distanceToConeOrigin = (point - coneOrigin).magnitude;
            if (distanceToConeOrigin < maxDistance)
            {
                var pointDirection = point - coneOrigin;
                var angle = Vector3.Angle(coneDirection, pointDirection);
                if (angle < maxAngle)
                    return true;
            }
            return false;
        }
        private IEnumerator OnTargetEventTrigger(GameObject target)
        {
            OnCharacterSeenEvent.Invoke(target);
            yield return new WaitUntil(() => (!seenCharacters.Contains(target)));
            _tempbool = false;
            OnCharacterLostEvent.Invoke(target);
        }
    }
}