using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using UnityEditor.SceneManagement;
using System.Net.Http.Headers;

public partial class ExportScene : EditorWindow
{
    string savePath = "Assets/";
    static Dictionary<string, Func<Component, IComponentData>> GetComponentData = new Dictionary<string, Func<Component, IComponentData>>();

    [MenuItem("SakuraFrame-Tools/ExportScene")]
    static public void Main()
    {
        Init();

        GetWindow<ExportScene>().Show();
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
        EditorGUILayout.TextField(savePath);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Export"))
        {
            ExportActiveScene();
        }
    }



    void ExportActiveScene()
    {
        string _savePath = $"{savePath}/{GetSceneName()}.SFSce";
        GameObject[] gameObjects = GetAllSceneObject();

        JsonData jsonData = new JsonData();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            List<JsonData> components = new List<JsonData>();
            foreach (var component in gameObjects[i].GetComponents<Component>())
            {
                try
                {
                    components.Add(GetJson(GetComponentData[component.GetType().ToString()](component)));
                }
                catch
                {
                    Debug.Log($"Is not allow Type {component.GetType()}");
                }
            }
            if (components.Count > 0)
            {
                JsonData _jo = new JsonData();
                _jo["Name"] = gameObjects[i].name;
                JsonData _jc = new JsonData();
                for(int c = 0; c < components.Count; c++)
                {
                    _jc.Add(components[c]);
                }
                _jo["Components"] = _jc;
                jsonData.Add(_jo);
            }
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
