using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponInspector : Editor
{
    SerializedProperty autoPlaceShootPointsProperty;
    SerializedProperty shootPointsProperty;
    private void OnEnable()
    {
        autoPlaceShootPointsProperty = serializedObject.FindProperty("autoPlaceShootPositions");
        shootPointsProperty = serializedObject.FindProperty("shootPositions");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(autoPlaceShootPointsProperty);
        if (autoPlaceShootPointsProperty.boolValue == false)
        {
            EditorGUILayout.PropertyField(shootPointsProperty);
        }
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

    }
}
