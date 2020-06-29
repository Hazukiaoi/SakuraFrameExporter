using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using UnityEditor.SceneManagement;
using System.Net.Http.Headers;
using UnityEngine.TestTools.Utils;

public partial class ExportScene : EditorWindow
{
    string savePath = "Assets/SamScen/";
    static Dictionary<string, Func<Component, IComponentData>> GetComponentData = new Dictionary<string, Func<Component, IComponentData>>();

    static ExportScene window;

    [MenuItem("SakuraFrame-Tools/ExportScene")]
    static public void Main()
    {
        Init();

        window = GetWindow<ExportScene>();
        window.Show();
    }

    static void Init()
    {
        GetComponentData.Add("UnityEngine.Transform",       (Component) => { return (TransformData)Component; });
        GetComponentData.Add("UnityEngine.Camera",          (Component) => { return (CameraData)Component; });
        GetComponentData.Add("UnityEngine.Light",           (Component) => { return (LightData)Component; });
        GetComponentData.Add("UnityEngine.MeshFilter",      (Component) => { return (MeshFilterData)Component; });
        GetComponentData.Add("UnityEngine.MeshRenderer",    (Component) => { return (MeshRendererData)Component; });
    }

    private void OnDestroy()
    {
        GetComponentData.Clear();
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Path(Without file name:)");
        savePath = EditorGUILayout.TextField(savePath);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Export"))
        {
            //如果组件数据获取字典为空，则重新初始化
            if (GetComponentData == null) Init();

            ExportActiveScene();
            ExportAssetsInfo();

            AssetDatabase.Refresh();
        }
    }

    //递归获取和创建树状节点的JsonData
    void GetJsonData(ref JsonData jsonData, GameObject gameObject)
    {
        List<JsonData> components = new List<JsonData>();
        foreach (var component in gameObject.GetComponents<Component>())
        {
            try
            {
                components.Add(GetJson(GetComponentData[component.GetType().ToString()](component)));
            }
            catch(Exception e)
            {
                //Debug.Log($"Is not allow Type {component.GetType().ToString()}");
                Debug.LogWarning($"Type {component.GetType()} error {e}");
            }
        }
        if (components.Count > 0)
        {
            JsonData _jo = new JsonData();
            _jo["Name"] = gameObject.name;
            JsonData _jc = new JsonData();
            for (int c = 0; c < components.Count; c++)
            {
                _jc.Add(components[c]);
            }
            _jo["Components"] = _jc;
            jsonData.Add(_jo);
        }

        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            GetJsonData(ref jsonData, gameObject.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 导出打开的场景
    /// </summary>
    void ExportActiveScene()
    {
        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        string _savePath = $"{savePath}/{GetSceneName()}.SFSce";
        GameObject[] gameObjects = GetAllSceneObject();

        JsonData jsonData = new JsonData();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GetJsonData(ref jsonData, gameObjects[i]);
        }

        System.IO.File.WriteAllText(_savePath, jsonData.ToJson());
    }

    GameObject[] GetAllSceneObject()
    {
        var scene = EditorSceneManager.GetActiveScene();
        return scene.GetRootGameObjects();
    }

    string GetSceneName()
    {
        return EditorSceneManager.GetActiveScene().name;
    }
}
