using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTools
{
    public static Vector3 ToVector3XZ(this Vector2Int value)
    {
        return new Vector3(value.x, 0f, value.y);
    }
}
