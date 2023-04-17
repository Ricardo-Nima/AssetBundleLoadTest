using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Position : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.transform.localPosition.ToString() + "g" + gameObject.transform.position.ToString() + "  r: " + gameObject.transform.rotation.ToString());
        if (gameObject.transform.childCount > 0)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            GameObject tempPrefab = PrefabUtility.SaveAsPrefabAsset(child, Application.dataPath + "/" + gameObject.name + ".prefab");
            tempPrefab.transform.position = child.transform.position;
            tempPrefab.transform.rotation = child.transform.rotation;
            Instantiate(tempPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
