using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadFrom))]
public class LoadFromEditor : Editor
{
    //private string _importPath = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LoadFrom loadFrom = (LoadFrom)target;
        

        string oldPath = loadFrom.path;

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        string newPath = EditorGUILayout.TextField("Path", oldPath);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Browse"))
        {
            //string path = EditorUtility.OpenFolderPanel("Select Path", Application.dataPath, "");
            string path = EditorUtility.OpenFilePanel("Select Path", @"C:\Users\15151\UnityWorks\HKAA_L5\Assets", "structurescene,radscene");
            if (path.Length != 0)
            {
                newPath = path;
                //EditorPrefs.SetString("BuildPath", _importPath);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            var pathPropety = serializedObject.FindProperty("path");
            pathPropety.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();

    }
}

