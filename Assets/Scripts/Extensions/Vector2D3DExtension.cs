using System.Collections.Generic;
using UnityEngine;

public static class Vector2D3DExtension
{
    public static Vector2 To2D(this Vector3 v)  
    {  
        return new Vector2(v.x, v.y);
    }

    public static Vector3 To3D(this Vector2 v, float z = 0)  
    {  
        return new Vector3(v.x, v.y, z);
    }
}