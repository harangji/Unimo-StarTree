using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static float AngleInXZ(this Vector2 vector)
    {
        if (vector.magnitude < 10f * float.Epsilon) { return 0f; }

        float angle = Mathf.Asin(Mathf.Abs(vector.y) / vector.magnitude);
        if (vector.y >= 0)
        {
            if (vector.x >= 0) { return angle; }
            else { return Mathf.PI - angle; }
        }
        else
        {
            if (vector.x >= 0) { return 2 * Mathf.PI - angle; }
            else { return Mathf.PI + angle; }
        }
    }
    public static float AngleInXZ(this Vector3 vector)
    {
        Vector2 vec2 = new Vector2(vector.x, vector.z);
        return AngleInXZ(vec2);
    }
    public static bool IsCCRot(this Vector3 vector, Vector3 othervec)
    {
        float y = Vector3.Cross(vector, othervec).y;
        return y >= 0;
    }
}
