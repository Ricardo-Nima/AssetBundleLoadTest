using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class UnpackInst : EditorWindow
{
    [MenuItem("Tools/Unpack")]
    private static void OpenBuildEditorWindow()
    {
        //UnpackInst buildWindow = GetWindowWithRect<UnpackInst>(new Rect(0, 0, 600, 620), false, "HKIA BuildTool");
        UnpackInst buildWindow = GetWindow<UnpackInst>("UnpackInst");
        buildWindow.Show();
    }
    GameObject go;
    SceneAsset sa;
    private void OnGUI()
    {
        sa = EditorGUILayout.ObjectField("Scene", sa, typeof(SceneAsset), true) as SceneAsset;
        go = EditorGUILayout.ObjectField("GameObject", go, typeof(GameObject), true) as GameObject;
        
        if (GUILayout.Button("build"))
        {
            Scene scene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sa));
            var instgo = Instantiate(go);
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(instgo))
            {
                PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                Debug.Log("1 done");
            }
            if (PrefabUtility.IsPartOfPrefabAsset(instgo))
            {
                Debug.Log("2 done");
            }
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(go))
            {
                PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                Debug.Log("3 done");
            }
            if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                Debug.Log("4 done");
            }
        }
    }
}
