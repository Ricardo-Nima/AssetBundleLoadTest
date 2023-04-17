using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Unpack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject));
        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
        {
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Debug.Log("done");
        }
        if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            Debug.Log("2 done");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
