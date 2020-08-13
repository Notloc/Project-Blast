using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Converts the vector2 into a vector3, using the Y coordinate as Z instead.
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static Vector3 ToVector3Z(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0f, vector2.y);
    }

    /// <summary>
    /// Converts the vector2 into a vector3, using the Y coordinate as Z instead.
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static Vector3Int ToVector3IntZ(this Vector2Int vector2)
    {
        return new Vector3Int(vector2.x, 0, vector2.y);
    }

    /// <summary>
    /// Returns the vector with Y = 0
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static Vector3 Flatten(this Vector3 vector3)
    {
        return new Vector3(vector3.x, 0f, vector3.z);
    }

    public static Quaternion Flatten(this Quaternion rotation)
    {
        Vector3 eulers = rotation.eulerAngles;
        eulers.x = 0f;
        return Quaternion.Euler(eulers);
    }
}
