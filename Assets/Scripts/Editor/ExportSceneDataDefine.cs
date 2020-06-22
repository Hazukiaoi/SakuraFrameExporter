using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;

public partial class ExportScene : EditorWindow
{
    static public JsonData GetJson(IComponentData component)
    {
        return component.ToJson();
    }

    public interface IComponentData
    {
        string ComponentName { get; }
        JsonData ToJson();
    }
    #region Transform
    public class TransformData : IComponentData
    {
        public string ComponentName { get => "Transform"; }
        public Vector3 position;
        public Quaternion rotaiotn;
        public Vector3 scale;
        public JsonData ToJson()
        {
            JsonData jd = new JsonData();
            jd["ComponentName"] = ComponentName;

            jd["Position"] = new JsonData();
            jd["Position"]["X"] = position.x;
            jd["Position"]["Y"] = position.y;
            jd["Position"]["Z"] = position.z;

            jd["Rotation"] = new JsonData();
            jd["Rotation"]["X"] = rotaiotn.x;
            jd["Rotation"]["Y"] = rotaiotn.y;
            jd["Rotation"]["Z"] = rotaiotn.z;
            jd["Rotation"]["W"] = rotaiotn.w;

            jd["Scale"] = new JsonData();
            jd["Scale"]["X"] = scale.x;
            jd["Scale"]["Y"] = scale.y;
            jd["Scale"]["Z"] = scale.z;
            return jd;
        }

        public static implicit operator TransformData(Transform d)
        {
            return new TransformData() { position = d.position, rotaiotn = d.rotation, scale = d.localScale };
        }
    }
    #endregion

    #region Camera
    public class CameraData : IComponentData
    {
        public string ComponentName => "Camera";

        public float Size;
        public float FOV;
        public float FarPlane;
        public float NearPlane;

        public float Aspect;
        public int Layer;

        public JsonData ToJson()
        {
            JsonData jd = new JsonData();
            jd["Size"] = Size;
            jd["FOV"] = FOV;
            jd["FarPlane"] = FarPlane;
            jd["NearPlane"] = NearPlane;
            jd["Aspect"] = Aspect;
            jd["Layer"] = Layer;
            return jd;
        }

        public static implicit operator CameraData(Camera d)
        {
            return new CameraData()
            {
                Size = d.orthographicSize,
                FOV = d.fieldOfView,
                FarPlane = d.farClipPlane,
                NearPlane = d.nearClipPlane,
                Aspect = d.aspect,
                Layer = (int)d.depth
            };
        }
    }
    #endregion

    #region Light
    class LightData : IComponentData
    {
        public enum LightType
        {
            LIGHT_DIRECTTIONAL,
	        LIGHT_POINT,
	        LIGHT_SPOT,
        };
        public string ComponentName => "Light";

        public float Intensity;
        public float Range;

        public int lightType;

        public Color color;

        public JsonData ToJson()
        {
            JsonData jd = new JsonData();
            jd["Intensity"] = Intensity;
            jd["Range"] = Range;
            jd["LightType"] = lightType;
            jd["Color"] = new JsonData();
            jd["Color"]["R"] = color.r;
            jd["Color"]["G"] = color.g;
            jd["Color"]["B"] = color.b;
            jd["Color"]["A"] = 1.0f;

            return jd;

        }

        public static implicit operator LightData(Light d)
        {
            int lt = 0;
            switch(d.type)
            {
                case UnityEngine.LightType.Directional:
                    lt = 0;
                    break;
                case UnityEngine.LightType.Point:
                    lt = 1;
                    break;
                case UnityEngine.LightType.Spot:
                    lt = 2;
                    break;
            }

            return new LightData()
            {
                Intensity = d.intensity,
                Range = d.range,
                lightType = lt,
                color = d.color
            };
        }
    }

    #endregion

    #region MeshFilter
    public class MeshFilterData : IComponentData
    {
        public string ComponentName => "MeshFilter";

        int mesh;

        public JsonData ToJson()
        {
            JsonData jd = new JsonData();
            jd["Mesh"] = mesh;
            return jd;
        }

        public static implicit operator MeshFilterData(MeshFilter d)
        {
            return new MeshFilterData()
            {
                mesh = d.sharedMesh.GetInstanceID()
            };
        }
    }
    #endregion

    #region MeshRenderer
    public class MeshRendererData : IComponentData
    {
        string IComponentData.ComponentName => "MeshRenderer";

        int material;

        JsonData IComponentData.ToJson()
        {
            JsonData jd = new JsonData();
            jd["Material"] = material;
            
            return jd;
        }

        public static implicit operator MeshRendererData(MeshRenderer d)
        {
            return new MeshRendererData() { material = d.sharedMaterial.GetInstanceID() };
        }
    }
    #endregion

}
