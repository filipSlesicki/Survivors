using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponData))]
[CanEditMultipleObjects]
public class WeaponDataInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponData owner = target as WeaponData;

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
        if( GUILayout.Button("Copy stats to scriptable"))
        {
            owner.CopyToGunStats();
        }

    }
}
