using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using LitJson;

public partial class ExportScene : EditorWindow
{
    string meshAssetPath = "Mesh/";
    string textureAssetPath = "Textures/";

    static Dictionary<int, string> assetInfo = new Dictionary<int, string>();

    void AssetExport(Mesh mesh)
    {
        if(assetInfo.ContainsKey(mesh.GetInstanceID())) return;

        string filename = mesh.GetInstanceID().ToString() + ".dat";
        string _savePath = savePath + meshAssetPath;
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        _savePath += filename;

        //写入
        using (FileStream fs = new FileStream(_savePath, FileMode.OpenOrCreate))
        {
            //写入顶点总数
            var vCount = ConvertToByte.ToByte(mesh.vertexCount);
            Debug.Log(vCount.Length);
            fs.Write(vCount, 0, vCount.Length);

            bool hadColor = mesh.colors.Length == mesh.vertexCount;
            bool hadUV1 = mesh.uv2.Length == mesh.vertexCount;

            //写入顶点数据
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                var ve = ConvertToByte.ToByte(mesh.vertices[i]);
                var co = hadColor ? ConvertToByte.ToByte(mesh.colors[i]) : ConvertToByte.ToByte(Color.white);
                var nr = ConvertToByte.ToByte(mesh.normals[i]);
                var ta = ConvertToByte.ToByte(mesh.tangents[i]);
                var uv0 = ConvertToByte.ToByte(mesh.uv[i]);
                var uv1 = hadUV1 ? ConvertToByte.ToByte(mesh.uv2[i]) : ConvertToByte.ToByte(Vector2.zero);
                fs.Write(ve, 0, ve.Length);
                fs.Write(co, 0, co.Length);
                fs.Write(nr, 0, nr.Length);
                fs.Write(ta, 0, ta.Length);
                fs.Write(uv0, 0, uv0.Length);
                fs.Write(uv1, 0, uv1.Length);
            }

            //写入索引数据
            var trisCount = ConvertToByte.ToByte(mesh.triangles.Length);
            var tris = ConvertToByte.ToByte(mesh.triangles);

            fs.Write(trisCount, 0, trisCount.Length);
            fs.Write(tris, 0, tris.Length);

            Vector3 bCenter = mesh.bounds.center;
            Vector3 bSize = mesh.bounds.size;
            Vector3 bMin = mesh.bounds.min;
            Vector3 bMax = mesh.bounds.max;

            var byteCenter = ConvertToByte.ToByte(bCenter);
            var byteSize = ConvertToByte.ToByte(bSize);
            var byteMin = ConvertToByte.ToByte(bMin);
            var byteMax = ConvertToByte.ToByte(bMax);

            fs.Write(byteCenter, 0, byteCenter.Length);
            fs.Write(byteSize, 0, byteSize.Length);
            fs.Write(byteMin, 0, byteMin.Length);
            fs.Write(byteMax, 0, byteMax.Length);

            Debug.Log(bCenter.ToString("F5"));
            Debug.Log(bSize.ToString("F5"));
            Debug.Log(bMin.ToString("F5"));
            Debug.Log(bMax.ToString("F5"));

        }

        Debug.Log(mesh.vertexCount);
        Debug.Log(mesh.triangles.Length);

        //添加已经保存的文件路径信息
        assetInfo.Add(mesh.GetInstanceID(), _savePath);
    }

    void AssetExport(Material material)
    {

    }

    void AssetExport(Texture2D t2d)
    {
        //处理保存路径
        if (assetInfo.ContainsKey(t2d.GetInstanceID())) return;

        string filename = t2d.GetInstanceID().ToString() + ".dat";
        string _savePath = savePath + textureAssetPath;
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        _savePath += filename;

        int w = t2d.width;
        int h = t2d.height;
        int c = 3;

        byte[] bytes = new byte[12 + w * h * c];

        //记录图片信息
        var bw = ConvertToByte.ToByte(w);
        var bh = ConvertToByte.ToByte(h);
        var bc = ConvertToByte.ToByte(c);

        for (int i = 0; i < 4; i++)
        {
            bytes[i] = bw[i];
            bytes[i + 4] = bh[i];
            bytes[i + 8] = bc[i];
        }


        int cid = 12;
        for (int y = h - 1; y >= 0; y--)
        {
            for (int x = 0; x < w; x++)
            {
                var col = t2d.GetPixel(x, y);

                bytes[cid] = (byte)(int)(col.r * 255.99f);
                bytes[cid + 1] = (byte)(int)(col.g * 255.99f);
                bytes[cid + 2] = (byte)(int)(col.b * 255.99f);

                cid += 3;
            }
        }
        File.WriteAllBytes(_savePath, bytes);

        //更新资源信息
        assetInfo.Add(t2d.GetInstanceID(), _savePath);
    }

    /// <summary>
    /// 保存场景资源索引
    /// </summary>
    void ExportAssetsInfo()
    {
        JsonData jsonData = new JsonData();
        string _savePath = $"{savePath}/{GetSceneName()}_AssetsInfo.SFSce";
        foreach (var info in assetInfo)
        {
            JsonData _assInfo = new JsonData();
            _assInfo["Name"] = info.Key;
            _assInfo["Path"] = info.Value;
            jsonData.Add(_assInfo);
        }

        File.WriteAllText(_savePath, jsonData.ToJson());
    }
}
