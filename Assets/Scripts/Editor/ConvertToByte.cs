using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ConvertToByte
{
    static public byte[] ToByte(bool data)
    {
        unsafe
        {
            return new byte[] { data ? (byte)0x01 : (byte)0x00 };
            return new byte[] { *(byte*)&data };
        }
    }

    static public byte[] ToByte(float data)
    {
        unsafe
        {
            byte* pdata = (byte*)&data;
            byte[] byteArray = new byte[sizeof(float)];
            for (int i = 0; i < sizeof(float); ++i)
                byteArray[i] = *pdata++;
            return byteArray;
        }
    }

    static public byte[] ToByte(int data)
    {
        unsafe
        {
            byte* pdata = (byte*)&data;
            byte[] byteArray = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); ++i)
                byteArray[i] = *pdata++;
            return byteArray;
        }
    }

    static public byte[] ToByte(int[] data)
    {
        unsafe
        {
            fixed (int* ptrf = data)
            {
                int itemSize = sizeof(int);
                byte[] byteArray = new byte[itemSize * data.Length];
                byte* bytes = (byte*)ptrf;
                for (int i = 0; i < data.Length * itemSize; ++i)
                {
                    byteArray[i] = *bytes++;
                }
                return byteArray;
            }
        }
    }

    static public byte[] ToByte(Vector2 data)
    {
        List<byte> result = new List<byte>(2 * sizeof(float));

        result.AddRange(ToByte(data.x));
        result.AddRange(ToByte(data.y));

        return result.ToArray();
    }

    static public byte[] ToByte(Vector3 data)
    {
        List<byte> result = new List<byte>(3 * sizeof(float));

        result.AddRange(ToByte(data.x));
        result.AddRange(ToByte(data.y));
        result.AddRange(ToByte(data.z));

        return result.ToArray();
    }

    static public byte[] ToByte(Vector4 data)
    {
        List<byte> result = new List<byte>(4 * sizeof(float));

        result.AddRange(ToByte(data.x));
        result.AddRange(ToByte(data.y));
        result.AddRange(ToByte(data.z));
        result.AddRange(ToByte(data.w));

        return result.ToArray();
    }

    static public byte[] ToByte(Vector2[] data)
    {
        List<byte> result = new List<byte>(data.Length * 2 * sizeof(float));
        for (int i = 0; i < data.Length; i++)
        {
            var bsx = ToByte(data[i].x);
            var bsy = ToByte(data[i].y);
            result.AddRange(bsx);
            result.AddRange(bsy);
        }
        return result.ToArray();
    }

    static public byte[] ToByte(Vector3[] data)
    {
        List<byte> result = new List<byte>(data.Length * 3 * sizeof(float));
        for (int i = 0; i < data.Length; i++)
        {
            var bsx = ToByte(data[i].x);
            var bsy = ToByte(data[i].y);
            var bsz = ToByte(data[i].z);
            result.AddRange(bsx);
            result.AddRange(bsy);
            result.AddRange(bsz);
        }
        return result.ToArray();
    }

    static public byte[] ToByte(Color[] data)
    {
        List<byte> result = new List<byte>(data.Length * 4 * sizeof(float));
        for (int i = 0; i < data.Length; i++)
        {
            var bsx = ToByte(data[i].r);
            var bsy = ToByte(data[i].g);
            var bsz = ToByte(data[i].b);
            var bsw = ToByte(data[i].a);
            result.AddRange(bsx);
            result.AddRange(bsy);
            result.AddRange(bsz);
            result.AddRange(bsw);
        }
        return result.ToArray();
    }

    static public byte[] ToByte(Vector4[] data)
    {
        List<byte> result = new List<byte>(data.Length * 4 * sizeof(float));
        for (int i = 0; i < data.Length; i++)
        {
            var bsx = ToByte(data[i].x);
            var bsy = ToByte(data[i].y);
            var bsz = ToByte(data[i].z);
            var bsw = ToByte(data[i].w);
            result.AddRange(bsx);
            result.AddRange(bsy);
            result.AddRange(bsz);
            result.AddRange(bsw);
        }
        return result.ToArray();
    }
}
