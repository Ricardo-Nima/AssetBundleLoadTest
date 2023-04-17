using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HKIABuildTools.Scripts;
using System.Linq;

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
                ImportRADSceneInOne(directoryInfo);
            }
            else
            {
                ImportStructureScene(directoryInfo);
            }
        }
    }
    void ImportRADSceneInOne(DirectoryInfo directoryInfo)
    {
        FileInfo[] jsonFiles = directoryInfo.GetFiles("*.json");
        if (jsonFiles.Length == 0)
            return;
        var jsonFileInfo = jsonFiles[0];
        string jsonContext = File.ReadAllText(jsonFileInfo.FullName);
        SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonContext);
        SceneObjectData[] objectList = sceneData.ObjectList;

        DirectoryInfo allInOneDir = new DirectoryInfo(directoryInfo.FullName + "/AssetBundleInone");
        if (allInOneDir == null)
            return;
        FileInfo[] allInOneFiles = allInOneDir.GetFiles();
        foreach (var file in allInOneFiles)
        {
            if (file.Name.EndsWith(".meta"))
                continue;
            byte[] abInOne = File.ReadAllBytes(file.FullName);
            int totalBytes = abInOne.Length;
            for (int i = 0; i < objectList.Length; i++)
            {
                var curObject = objectList[i];
                byte[] abBytes = abInOne.Skip(curObject.ABOffset).Take(curObject.ABSize).ToArray();
                Debug.Log($"curObject.name<{curObject.PrefabName}>, ABOffset<{curObject.ABOffset}>, size<{curObject.ABSize}>");
                AssetBundle assetBundle = AssetBundle.LoadFromMemory(abBytes);
                Object[] objects = assetBundle.LoadAllAssets();
                if (objects.Length != 1)
                {
                    Debug.LogError($"This AssetBundle has multi objects: <{assetBundle.name}>");
                    return;
                }
                GameObject go = objects[0] as GameObject;
                if (go == null)
                {
                    Debug.LogError($"Object <{assetBundle.name}> cannot convert to GameObject");
                    return;
                }
                go.transform.position = curObject.Position;
                go.transform.rotation = curObject.Rotation;
                go.transform.localScale = curObject.LossyScale;
                Instantiate(go);
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
                    Instantiate(pref);
            }
        }
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
