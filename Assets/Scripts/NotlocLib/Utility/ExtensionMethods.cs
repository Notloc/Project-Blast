using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notloc.Utility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the Vector2 into a Vector3, using the Y coordinate as Z instead.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector3 ToVector3Z(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }

        /// <summary>
        /// Converts the Vector2Int into a Vector3Int, using the Y coordinate as Z instead.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector3Int ToVector3IntZ(this Vector2Int vector2)
        {
            return new Vector3Int(vector2.x, 0, vector2.y);
        }

        /// <summary>
        /// Returns the Vector with Y = 0
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3 Flatten(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0f, vector3.z);
        }







        public static Vector2 Swap(this Vector2 vector)
        {
            return new Vector2(vector.y, vector.x);
        }

        public static Vector2Int Swap(this Vector2Int vector)
        {
            return new Vector2Int(vector.y, vector.x);
        }










        /// <summary>
        /// Zeroes the quaternion's x rotation.
        /// In the right reference frame this "flattens" the rotation to be on plane.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion Flatten(this Quaternion rotation)
        {
            Vector3 eulers = rotation.eulerAngles;
            eulers.x = 0f;
            return Quaternion.Euler(eulers);
        }













        public static RectTransform RectTransform(this Transform t)
        {
            return (RectTransform)t;
        }



    }
}