using Unity.Mathematics;
using UnityEngine;

public static class GeometricHelpers{
    public static bool IsInRange(float3 point1, float3 point2, float range)
    {
        float distance = math.distance(point1, point2);
        return distance < range;
    }


    public static float CalculateAngleToPositionXZ(float3 point1, float3 point2)
    {
        float dx = point2.x - point1.x;
        float dz = point2.z - point1.z;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dx, dz);
        return NormalizeDegrees(angle);
    }

    public static Vector3 AngleToVector(float angleInDegrees)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians);
        float z = Mathf.Cos(radians);
        return new Vector3(x, 0, z);
    }

    public static float FindAngleDelta(float angle1, float angle2)
    {
        angle1 = NormalizeDegrees(angle1);
        angle2 = NormalizeDegrees(angle2);
        
        float delta = angle2 - angle1;
        if (delta > 180f) 
        {
            delta -= 360f;
        } 
        else if (delta < -180f) 
        {
            delta += 360f;
        }
        
        return delta;
    }

    public static float NormalizeDegrees(float degrees)
    {
        degrees = degrees % 360;
        if (degrees < 0)
        {
            degrees += 360;
        }
        return degrees;
    }
}

    