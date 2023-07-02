using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SetWeaponStatsWindow : EditorWindow
{
    SerializedObject so;
    public List<WeaponData> datas;


     [MenuItem("Tools/Weapon Stats Setter")]
    public static void ShowWindow()
    {
        GetWindow<SetWeaponStatsWindow>();
    }
    private void OnEnable()
    {
        ScriptableObject target = this;
        so = new SerializedObject(target);
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Set stats tool");
        so.Update();
        SerializedProperty stringsProperty = so.FindProperty("datas");

        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Copy stats to scriptable"))
        {
            CopyStats();
        }
        if (GUILayout.Button("Clear stats to scriptable"))
        {
            ClearStats();
        }
    }

    void CopyStats()
    {
        foreach (var weapon in datas)
        {
            weapon?.CopyToGunStats();
        }
    }

    void ClearStats()
    {
        foreach (var weapon in datas)
        {
            weapon?.ClearStatsData();
        }
    }

}
