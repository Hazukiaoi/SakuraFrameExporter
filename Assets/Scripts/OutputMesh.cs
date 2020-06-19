using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.MemoryMappedFiles;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class OutputMesh : MonoBehaviour
{
    int[] imgs = new int[32];

    public byte[] ObjectToBytes(object obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {

            // //以二进制格式将对象或整个连接对象图形序列化和反序列化。
            IFormatter formatter = new BinaryFormatter();
            //把字符串以二进制放进memStream中
            formatter.Serialize(ms, obj);
            //返回从其创建此流的无符号字节数组。 是会返回所有分配的字节，不管用没用到。
            ////返回无符号字节数组 ，无符号字节数组 其实就是byte(0~255),有符号字节sbyte(-128~127)
            return ms.GetBuffer();
        }
    }


    public Mesh mesh;
    public string path = "";


    [ContextMenu("ExportMesh")]
    public void ExportMesh()
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
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

            /*
            var vex = ConvertToByte.ToByte(mesh.vertices);
            var nomrals = ConvertToByte.ToByte(mesh.normals);
            var tangents = ConvertToByte.ToByte(mesh.tangents);
            var uv = ConvertToByte.ToByte(mesh.uv);
            fs.Write(vex, 0, vex.Length);
            fs.Write(nomrals, 0, nomrals.Length);
            fs.Write(tangents, 0, tangents.Length);

            //判断如果存在顶点色，则写入
            bool hadColor = mesh.uv2.Length == mesh.vertexCount;
            fs.Write(ConvertToByte.ToByte(hadColor), 0, 1);
            if(hadColor)
            {
                var colors = ConvertToByte.ToByte(mesh.colors);
                fs.Write(colors, 0, colors.Length);
            }

            fs.Write(uv, 0, uv.Length);

            //判断UV2的存在并写入数据
            //如果UV2存在则直接写入
            bool hadUV2 = mesh.uv2.Length == mesh.vertexCount;
            fs.Write(ConvertToByte.ToByte(hadUV2), 0, 1);
            if(hadUV2)
            {
                var uv2 = ConvertToByte.ToByte(mesh.uv2);
                fs.Write(uv2, 0, uv2.Length);
            }

            var trisCount = ConvertToByte.ToByte(mesh.triangles.Length);
            var tris = ConvertToByte.ToByte(mesh.triangles);

            fs.Write(trisCount, 0, trisCount.Length);
            fs.Write(tris, 0, tris.Length);
            */
        }
        Debug.Log(mesh.vertexCount);
        Debug.Log(mesh.triangles.Length);

        return;
        //for(int i = 0; i < imgs.Length; i++)
        //{
        //    imgs[i] = i;
        //}

        //byte[] bytes = ObjectConvertToByte.ToBytes(imgs);

        float[] fls = new float[32];
        for(int i = 0; i < 32; i++)
        {
            fls[i] = i;
        }

        byte[] fbs = new byte[sizeof(float) * 32];
        unsafe
        {
            fixed (float* ptrf = fls)
            {
                byte* bytes = (byte*)ptrf;
                for (int i = 0; i < 32 * sizeof(float); ++i)
                {
                    fbs[i] = *bytes++;
                }
            }
        }

        using (FileStream fs = new FileStream("Assets/Save.dat", FileMode.OpenOrCreate))
        {
            fs.Write(fbs, 0, fbs.Length);
        }


        //imgs[0] = 1.0f;
        //imgs[1] = 3.0f;

        //unsafe
        //{
        //    fixed(float* ptr = &imgs[0])
        //    {
        //        byte[] bytes = null;

        //    }
        //}
    }

}
