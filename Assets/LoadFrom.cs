using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFrom : MonoBehaviour
{
    [SerializeField]
    public string path;
    // Start is called before the first frame update
    void Start()
    {
        if (path != null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if (directoryInfo == null)
            {
                Debug.LogError("no such dir: " + directoryInfo.FullName);
                return;
            }
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Name.EndsWith(".radscene"))
            {
                ImportRADScene(directoryInfo);
            }
            else
            {
                ImportStructureScene(directoryInfo);
            }
        }
    }
    void ImportRADScene(DirectoryInfo directoryInfo)
    {

        Dictionary<string, AssetBundle> abMap = new Dictionary<string, AssetBundle>();

        //遍历PrefabAB下的所有物体，并还原
        DirectoryInfo prefabDir = new DirectoryInfo(directoryInfo.FullName + "/AssetBundle");

        // load main ab
        AssetBundle mainAB = AssetBundle.LoadFromFile(prefabDir + "/AssetBundle");

        // only one asset in main ab, which name is "AssetBundleManifest"
        AssetBundleManifest abm = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] abLists = abm.GetAllAssetBundles();

        // first, load all ab into memory

        foreach (var abName in abLists)
        {
            Debug.Log(abName);
            AssetBundle curAB = AssetBundle.LoadFromFile(prefabDir + "/" + abName);
            abMap[abName] = curAB;
        }

        // second, load all gameobject

        foreach (var abName in abLists)
        {
            Object[] objLists = abMap[abName].LoadAllAssets();
            foreach (Object obj in objLists)
            {
                GameObject pref = obj as GameObject;
                if (pref != null)
                {
                    GameObject go = Instantiate(pref);
                }
            }
        }

        

        //LoadFromManifest
    }
    IEnumerator LoadFromManifest()
    {


        yield return null;
    }
    void ImportStructureScene(DirectoryInfo directoryInfo)
    {
        Dictionary<string, AssetBundle> abMap = new Dictionary<string, AssetBundle>();

        //遍历AB下的所有物体，并还原
        DirectoryInfo abDir = new DirectoryInfo(directoryInfo.FullName + "/AB");

        // load main ab
        AssetBundle mainAB = AssetBundle.LoadFromFile(abDir + "/AB");

        // only one asset in main ab, which name is "AssetBundleManifest"
        AssetBundleManifest abm = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] abLists = abm.GetAllAssetBundles();

        // first, load all ab into memory

        foreach (var abName in abLists)
        {
            Debug.Log(abName);
            AssetBundle curAB = AssetBundle.LoadFromFile(abDir + "/" + abName);
            abMap[abName] = curAB;
        }

        // second, load all gameobject

        foreach (var abName in abLists)
        {
            Object[] objLists = abMap[abName].LoadAllAssets();
            foreach (Object obj in objLists)
            {
                GameObject pref = obj as GameObject;
                if (pref != null)
                {
                    GameObject go = Instantiate(pref);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
