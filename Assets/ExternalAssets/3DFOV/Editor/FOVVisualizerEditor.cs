using System.Collections.Generic;
using System.Reflection;
using ExternalAssets._3DFOV.Scripts;
using UnityEditor;
using UnityEngine;

namespace ExternalAssets._3DFOV.Editor
{
    [CustomEditor(typeof(FOVVisualizer)), CanEditMultipleObjects]
    public class FOVVisualizerEditor : UnityEditor.Editor
    {
        private FOVVisualizer fovV;
        private FOVVisualizer.GizmoType memGizType;
        private FieldOfView3D.DetectionType memDetType;
        private List<p_FieldCondition> fieldConditions;
        private List<p_BoolCondition> boolConditions;
        public void OnEnable()
        {
            fovV = (FOVVisualizer)target;

            fieldConditions = new List<p_FieldCondition>();
            boolConditions = new List<p_BoolCondition>();
            SetFieldCondition();
        }

        private void SetFieldCondition()
        {
            ShowOnBool("wireframeActive", true, "wireframeColor");
            ShowOnBool("viewAllRaycastLines", true, "raycastColor");
            ShowOnBool("viewSeenObjectLines", true, "detectionColor");
            ShowOnBool("gizmosActive", true, "gizmoType");
            ShowOnBool("gizmosActive", true, "pointColor");

            ShowOnBool("p_viewSpherecasts", true, "viewSpherecasts");
            ShowOnBool("p_viewSpherecasts", true, "viewAllSpherecasts");
          
            if (!fovV.p_viewSpherecasts)
                ShowOnBool("p_viewSpherecasts", true, "spherecastColor");
            else
            {
                ShowOnBool("viewSpherecasts", true, "spherecastColor");
                ShowOnBool("p_viewSpherecasts", false, "spherecastColor");
            }
      
            if (fovV.gizmoType != FOVVisualizer.GizmoType.Point)
                ShowOnBool("gizmosActive", true, "pointSize");
            else
            {
                ShowOnEnum("gizmoType", "Cube", "pointSize"); //type1Var is only visible when type == Type1
                ShowOnEnum("gizmoType", "Disc", "pointSize"); //type1Var is only visible when type == Type1
                ShowOnEnum("gizmoType", "Ray", "pointSize"); //type1Var is only visible when type == Type1
                ShowOnEnum("gizmoType", "Sphere", "pointSize"); //type1Var is only visible when type == Type1
            }
        }
        private void ShowOnBool(string boolName, bool boolValue, string fieldName)
        {
            p_BoolCondition newBoolCondition = new p_BoolCondition()
            {
                p_boolName = boolName,
                p_boolValue = boolValue,
                p_fieldName = fieldName,
                p_isValid = true

            };
            boolConditions.Add(newBoolCondition);
        }
        private void ShowOnEnum(string enumFieldName, string enumValue, string fieldName)
        {
            p_FieldCondition newFieldCondition = new p_FieldCondition()
            {
                p_enumFieldName = enumFieldName,
                p_enumValue = enumValue,
                p_fieldName = fieldName,
                p_isValid = true

            };
            #region Validating Enum
            #region Validating "enumFieldName"
            newFieldCondition.p_errorMsg = "";
            FieldInfo enumField = target.GetType().GetField(newFieldCondition.p_enumFieldName);
            if (enumField == null)
            {
                newFieldCondition.p_isValid = false;
                newFieldCondition.p_errorMsg = "Could not find a enum-field named: '" + enumFieldName + "' in '" + target + "'. Make sure you have spelled the field name for the enum correct in the script '" + this.ToString() + "'";
            }
            #endregion
            #region Validating "enumValue"
            if (newFieldCondition.p_isValid)
            {
                var currentEnumValue = enumField.GetValue(target);
                var enumNames = currentEnumValue.GetType().GetFields();
                //var enumNames =currentEnumValue.GetType().GetEnumNames();
                bool found = false;
                foreach (FieldInfo enumName in enumNames)
                {
                    if (enumName.Name == enumValue)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    newFieldCondition.p_isValid = false;
                    newFieldCondition.p_errorMsg = "Could not find the enum value: '" + enumValue + "' in the enum '" + currentEnumValue.GetType().ToString() + "'. Make sure you have spelled the value name correct in the script '" + this.ToString() + "'";
                }
            }
            #endregion
            #region Validating "fieldName"
            if (newFieldCondition.p_isValid)
            {
                FieldInfo fieldWithCondition = target.GetType().GetField(fieldName);
                if (fieldWithCondition == null)
                {
                    newFieldCondition.p_isValid = false;
                    newFieldCondition.p_errorMsg = "Could not find the field: '" + fieldName + "' in '" + target + "'. Make sure you have spelled the field name correct in the script '" + this.ToString() + "'";
                }
            }
            #endregion
            if (!newFieldCondition.p_isValid)
            {
                newFieldCondition.p_errorMsg += "\nYour error is within the Custom Editor Script to show/hide fields in the inspector depending on the an Enum." +
                        "\n\n" + this.ToString() + ": " + newFieldCondition.ToStringFunction() + "\n";
            } 
            #endregion

            fieldConditions.Add(newFieldCondition);
        }
        public override void OnInspectorGUI()
        {
            if (memGizType != fovV.gizmoType)
            {
                fieldConditions = new List<p_FieldCondition>();
                boolConditions = new List<p_BoolCondition>();
                SetFieldCondition();
                memGizType = fovV.gizmoType;
            }

            if (memDetType != fovV.fov3D.detectionType)
            {
                fieldConditions = new List<p_FieldCondition>();
                boolConditions = new List<p_BoolCondition>();
                SetFieldCondition();
                memDetType = fovV.fov3D.detectionType;
            }

            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            var obj = serializedObject.GetIterator();
            if (obj.NextVisible(true))
            {
                // Loops through all visiuble fields
                do
                {
                    bool shouldBeVisible = true;
                    // Tests if the field is a field that should be hidden/shown due to the enum value
                    foreach (var fieldCondition in fieldConditions)
                    {
                        //If the fieldcondition isn't valid, display an error msg.
                        if (!fieldCondition.p_isValid)
                        {
                            Debug.LogError(fieldCondition.p_errorMsg);
                        }
                        else if (fieldCondition.p_fieldName == obj.name)
                        {
                            FieldInfo enumField = target.GetType().GetField(fieldCondition.p_enumFieldName);
                            var currentEnumValue = enumField.GetValue(target);
                            //If the enum value isn't equal to the wanted value the field will be set not to show
                            if (currentEnumValue.ToString() != fieldCondition.p_enumValue)
                                shouldBeVisible = false;
                            else
                            {
                                shouldBeVisible = true;
                                break;
                            }
                        }
                    }

                    foreach (var i in boolConditions)
                    {
                        if (i.p_fieldName == obj.name)
                        {
                            FieldInfo boolName = fovV.GetType().GetField(i.p_boolName);
                            var boolValue = boolName.GetValue(target);
                            if (boolValue.ToString() != i.p_boolValue.ToString())
                                shouldBeVisible = false;
                            else
                            {
                                shouldBeVisible = true;
                                break;
                            }
                        }
                    }
                    if (shouldBeVisible)
                        EditorGUILayout.PropertyField(obj, true);
                } while (obj.NextVisible(false));
            }
            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
        private class p_FieldCondition
        {
            public string p_enumFieldName { get; set; }
            public string p_enumValue { get; set; }
            public string p_fieldName { get; set; }
            public bool p_isValid { get; set; }
            public string p_errorMsg { get; set; }

            public string ToStringFunction()
            {
                return "'" + p_enumFieldName + "', '" + p_enumValue + "', '" + p_fieldName + "'.";
            }
        }

        private class p_BoolCondition
        {
            public string p_boolName { get; set; }
            public bool p_boolValue { get; set; }
            public string p_fieldName { get; set; }
            public bool p_isValid { get; set; }
            public string p_errorMsg { get; set; }

            public string ToStringFunction()
            {
                return "'" + p_boolName + "', '" + p_boolValue + "', '" + p_fieldName + "'.";
            }
        }
    }
}
