using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllShaderPro : MonoBehaviour
{
    public Material material;

    [ContextMenu("GetMatData")]
    public void GetMatData()
    {
        Color c = Color.cyan;
        JsonData _jcolor = new JsonData();
        _jcolor.Add((double)c.r);
        _jcolor.Add((double)c.g);
        _jcolor.Add((double)c.b);
        _jcolor.Add((double)c.a);
        Debug.Log(_jcolor.ToJson());

        return;


        Shader shader = material.shader;
        int pcount = shader.GetPropertyCount();
        List<string> _proName = new List<string>(pcount);
        List<UnityEngine.Rendering.ShaderPropertyType> _proType = new List<UnityEngine.Rendering.ShaderPropertyType>(pcount);
        for (int i = 0; i < pcount; i++)
        {
            //Debug.Log(shader.GetPropertyName(i) + "|" + shader.GetPropertyType(i));
            _proName.Add(shader.GetPropertyName(i));
            _proType.Add(shader.GetPropertyType(i));
        }
     
        for(int i = 0; i < _proType.Count; i++)
        {
            Debug.Log(shader.GetPropertyName(i) + "|" + shader.GetPropertyType(i));
            switch (_proType[i])
            {
                case UnityEngine.Rendering.ShaderPropertyType.Color:
                    Debug.Log(material.GetColor(_proName[i]));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Float:
                    Debug.Log(material.GetFloat(_proName[i]));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Range:
                    Debug.Log(material.GetFloat(_proName[i]));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Vector:
                    Debug.Log(material.GetFloat(_proName[i]));
                    break;
                case UnityEngine.Rendering.ShaderPropertyType.Texture:
                    Debug.Log(material.GetTextureOffset(_proName[i]));
                    Debug.Log(material.GetTextureScale(_proName[i]));
                    Debug.Log(material.GetTexture(_proName[i]));

                    Texture _t = material.GetTexture(_proName[i]);
                    var _ttype = _t.GetType();
                    Debug.Log(_ttype.ToString());
                    break;
            }
        }
    }

}
