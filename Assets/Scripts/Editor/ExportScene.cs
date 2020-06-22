using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using UnityEditor.SceneManagement;

public partial class ExportScene : EditorWindow
{
    string savePath = "Assets/";
    static Dictionary<Type, Func<Component, IComponentData>> GetComponentData = new Dictionary<Type, Func<Component, IComponentData>>();

    [MenuItem("SakuraFrame-Tools/ExportScene")]
    static public void Main()
    {
        GetComponentData.Add(typeof(Transform), (Component) => { Debug.Log("Trans"); return (TransformData)(Transform)Component; });
        GetWindow<ExportScene>().Show();
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
        List<JsonData> jsonDatas = new List<JsonData>(gameObjects.Length);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            List<Component> components = new List<Component>();
            foreach (var component in gameObjects[i].GetComponents<Component>())
            {
                components.Add(component);
                Debug.Log(component.GetType());
            }
        }
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
