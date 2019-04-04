using UnityEngine;

namespace ExtensionMethods
{
    public static class VectorExtensionMethods
    {
        public static Vector3 Map(this Vector3 target, System.Func<float, float> operation)
        {
            Vector3 result = new Vector3(operation(target.x), operation(target.y), operation(target.z));
            return result;
        }


        public static Vector3 Map(this Vector3 current, Vector3 other, System.Func<float, float, float> operation)
        {
            Vector3 result = new Vector3(operation(current.x, other.x), operation(current.y, other.y), operation(current.z, other.y));
            return result;
        }
    }
}