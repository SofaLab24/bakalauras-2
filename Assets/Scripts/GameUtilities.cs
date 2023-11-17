using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilities
{
    public static float VectorDifference(Vector3 a, Vector3 b )
    {
        float vec1 = a.y*a.y+a.x*a.x+a.z*a.z;
        float vec2 = b.y*b.y+b.x*b.x+b.z*b.z;

        return Mathf.Abs(vec1 - vec2);
    }
}
