using System;
using UnityEngine;

namespace HKIABuildTools.Scripts
{
    [Serializable]
    public struct SceneData
    {
        public string ProjectName;
        public string SceneName;
        public SceneObjectData[] ObjectList;

        public static SceneData NewDataScene(string projectName, string sceneName, SceneObjectData[] objectList)
        {
            SceneData data = new SceneData();
            data.ProjectName = projectName;
            data.SceneName = sceneName;
            data.ObjectList = objectList;

            return data;
        }
    }

    [Serializable]
    public struct SceneObjectData
    {
        public int ABOffset;
        public int ABSize;
        public string PrefabName;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LossyScale;
        public Vector3 BoundsMin;
        public Vector3 BoundsMax;
        public string ModifyMatID;
        public string objLibPath;
        public bool isActive;

        public bool isNotActive
        {
            get
            {
                return isActive;
            }
        }

        public void ToWorldSpace(Matrix4x4 worldSpaceMatrix)
        {
            Matrix4x4 localSpaceMatrix = Matrix4x4.identity;
            localSpaceMatrix.SetTRS(Position, Rotation, Vector3.one);
            Matrix4x4 newWorldSpaceMatrix = worldSpaceMatrix * localSpaceMatrix;

            Position = newWorldSpaceMatrix.MultiplyPoint(Vector3.zero);
            Rotation = newWorldSpaceMatrix.rotation;

            Vector3 size = BoundsMax - BoundsMin;
            Vector3 localSpaceCenter = size / 2 + BoundsMin;
            Vector3 worldSpaceCenter = worldSpaceMatrix.MultiplyPoint(localSpaceCenter);
            Bounds newBounds = new Bounds(worldSpaceCenter, size);
            BoundsMin = newBounds.min;
            BoundsMax = newBounds.max;
        }

        public void ToLocalSpace(Vector3 worldPos, Quaternion worldRotation, Vector3 worldScale, Bounds bounds, Matrix4x4 worldSpaceMatrix)
        {
            Matrix4x4 selfWorldSpaceMatrix = Matrix4x4.identity;
            selfWorldSpaceMatrix.SetTRS(worldPos, worldRotation, Vector3.one);

            Matrix4x4 localMatrix = worldSpaceMatrix.inverse;
            Matrix4x4 newlocalMatrix = localMatrix * selfWorldSpaceMatrix;

            Position = newlocalMatrix.MultiplyPoint(Vector3.zero);
            Rotation = newlocalMatrix.rotation;
            LossyScale = worldScale;

            Vector3 worldSpaceCenter = bounds.center;
            Vector3 localSpaceCenter = localMatrix.MultiplyPoint(worldSpaceCenter);
            Bounds newBounds = new Bounds(localSpaceCenter, bounds.size);
            BoundsMin = newBounds.min;
            BoundsMax = newBounds.max;
        }

        public void ToLocalSpace(GameObject obj, Matrix4x4 worldSpaceMatrix)
        {
            PrefabName = obj.name;
            Bounds bounds = SceneObjectData.CalcObjectMaxBounds(obj);
            ToLocalSpace(obj.transform.position, obj.transform.rotation, obj.transform.lossyScale, bounds, worldSpaceMatrix);
        }

        public static Bounds CalcObjectMaxBounds(GameObject targetObj)
        {
            Renderer[] renderers = targetObj.GetComponentsInChildren<Renderer>();
            int totalRenderers = renderers.Length;
            Bounds bounds = new Bounds();
            bool alreadySetFirst = false;
            for (int i = 0; i < totalRenderers; i++)
            {
                Renderer renderer = renderers[i];
                Bounds rendererBounds = renderer.bounds;
                if (alreadySetFirst)
                    bounds.Encapsulate(rendererBounds);
                else
                {
                    bounds = rendererBounds;
                    alreadySetFirst = true;
                }
            }

            return bounds;
        }
    }
}
