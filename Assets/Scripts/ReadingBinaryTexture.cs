using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingBinaryTexture : MonoBehaviour
{
    public Texture2D t2d;

    //[ContextMenu("Read")]
    //public void ReadTex()
    //{
    //    int w = 1000;
    //    int h = 1157;
    //    int c = 3;

        

       
    //    System.IO.File.WriteAllBytes("Assets/Save.png", t2d.EncodeToPNG());
    //}

    //GL从左上开始读取，并且XY对调
    //Unity从左下开始
    [ContextMenu("Save")]
    public void SaveTex()
    {
        int w = t2d.width;
        int h = t2d.height;
        int c = 3;

        byte[] bytes = new byte[12 + w * h * c];

        //记录图片信息
        var bw = ConvertToByte.ToByte(w);
        var bh = ConvertToByte.ToByte(h);
        var bc = ConvertToByte.ToByte(c);

        for(int i = 0; i < 4; i ++)
        {
            bytes[i] = bw[i];
            bytes[i + 4] = bh[i];
            bytes[i + 8] = bc[i];
        }


        int cid = 12;
        for(int y = h - 1; y >= 0; y--)
        {
            for(int x = 0; x < w; x++)
            {
                var col = t2d.GetPixel(x, y);

                bytes[cid] =        (byte)(int)(col.r * 255.99f);
                bytes[cid + 1] =    (byte)(int)(col.g * 255.99f);
                bytes[cid + 2] =    (byte)(int)(col.b * 255.99f);

                cid += 3;
            }
        }
        System.IO.File.WriteAllBytes("Assets/Save.dat", bytes);
    }

    [ContextMenu("Create")]
    public void CreateTEX()
    {
        Texture2D t2d = new Texture2D(512, 512);

        for(int i = 0; i < 512; i++)
        {
            for(int j = 0; j < 512; j++)
            {
                t2d.SetPixel(i, j, new Color(i / 512.0f, j / 512.0f, 0.0f));
            }
        }

        System.IO.File.WriteAllBytes("Assets/SaveTex.png", t2d.EncodeToPNG());
    }
}
