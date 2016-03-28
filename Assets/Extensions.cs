using UnityEngine;

public static class Extensions {
    public static double[] ToArray(this Vector3 vec) {
        return new double[] {vec.x, vec.y, vec.z};
    }
}