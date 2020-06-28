using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathEx : MonoBehaviour
{
    public enum AxisOrder
    {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX
    }

    //https://gamedev.stackexchange.com/questions/140579/euler-right-handed-to-quaternion-left-handed-conversion-in-unity
    //direction    Unity       GL 右手坐标系
    //----------------------------
    //right         x+         x-
    //up            y+         y+
    //forward       z+         z+
    static public Quaternion EulerToOpenGL(Vector3 eulerAngles, AxisOrder rotationOrder)
    {
        // BVH's x+ axis is Unity's left (x-)
        var xRot = Quaternion.AngleAxis(-eulerAngles.x, Vector3.left);
        // Unity & BVH agree on the y & z axes
        var yRot = Quaternion.AngleAxis(-eulerAngles.y, Vector3.up);
        var zRot = Quaternion.AngleAxis(-eulerAngles.z, Vector3.forward);

        switch (rotationOrder)
        {
            // Reproduce rotation order (no need for parentheses - it's associative)
            case AxisOrder.XYZ: return xRot * yRot * zRot;
            case AxisOrder.XZY: return xRot * zRot * yRot;
            case AxisOrder.YXZ: return yRot * xRot * zRot;
            case AxisOrder.YZX: return yRot * zRot * xRot;
            case AxisOrder.ZXY: return zRot * xRot * yRot;
            case AxisOrder.ZYX: return zRot * yRot * xRot;
        }

        return Quaternion.identity;
    }
}
