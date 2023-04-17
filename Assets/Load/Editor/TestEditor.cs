using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var test = target as Test;
        if (GUILayout.Button("Change"))
        {
            test.value = UnityEngine.Random.Range(1, 100);
        }
    }
}

