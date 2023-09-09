using GamePlayManagement.FieldOfView;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Editor
{
    [CustomEditor(typeof(BaseFieldOfView))]
    public class EditorFieldOfView : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var fieldOfView = (BaseFieldOfView) target;
            var objectTransform = fieldOfView.transform;
            var objectPosition = objectTransform.position;
            var objectAngles = objectTransform.eulerAngles;
            
            Handles.color = Color.white;
            Handles.DrawWireArc(objectPosition, Vector3.up, Vector3.forward, 360, fieldOfView.radius);

            
            var viewAngle1 = DirectionFromAngle(objectAngles.y, -fieldOfView.angle / 2);
            var viewAngle2 = DirectionFromAngle(objectAngles.y, fieldOfView.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(objectPosition, objectPosition + viewAngle1 * fieldOfView.radius);
            Handles.DrawLine(objectPosition, objectPosition + viewAngle2 * fieldOfView.radius);

            if (fieldOfView.isCustomerFound)
            {
                Handles.color = Color.blue;
                Handles.DrawLine(objectPosition, fieldOfView.foundObject.transform.position);
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
