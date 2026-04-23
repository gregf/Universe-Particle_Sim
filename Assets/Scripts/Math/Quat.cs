using Unity.Mathematics;
using UnityEngine;

public class Quat
{
    public float w;
    public float x;
    public float y;
    public float z;

    public Quat(float w, float x, float y, float z)
    {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Quat(Vector3 v)
    {
        this.w = 0;
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        // TODO (W6):
        // Build a pure quaternion from a vector:
        // p = (0, x, y, z)
        return;
    }

    public Quat (Vector3 axis, float angleRad)
    {
        MathLib.Normalize(axis);
        float halfAngle = angleRad * 0.5f;
        float w = Mathf.Cos(halfAngle);
        float x = axis.x * Mathf.Sin(halfAngle);
        float y = axis.y * Mathf.Sin(halfAngle);
        float z = axis.z * Mathf.Sin(halfAngle);
        // TODO (W6):
        // 1) normalise axis
        // 2) halfAngle = angleRad * 0.5f
        // 3) w = cos(halfAngle)
        // 4) x = axis.x * sin(halfAngle)
        // 5) y = axis.y * sin(halfAngle)
        // 6) z = axis.z * sin(halfAngle)
        return;
    }

    public static Quat operator * (Quat a, Quat b)
    {
        float W = a.w * b.w - (a.x * b.x + a.y * b.y + a.z * b.z);
        float X = a.w * b.x + b.w * a.x + (a.y * b.z - a.z * b.y);
        float Y = a.w * b.y + b.w * a.y + (a.z * b.x - a.x * b.z);
        float Z = a.w * b.z + b.w * a.z + (a.x * b.y - a.y * b.x);
        // TODO (W6):
        // Quaternion multiplication
        //
        // scalar part:
        // w' = a.w*b.w - (a.x*b.x + a.y*b.y + a.z*b.z)
        //
        // vector part:
        // x' = a.w*b.x + b.w*a.x + (a.y*b.z - a.z*b.y)
        // y' = a.w*b.y + b.w*a.y + (a.z*b.x - a.x*b.z)
        // z' = a.w*b.z + b.w*a.z + (a.x*b.y - a.y*b.x)
        //
        // return new Quat(w', x', y', z')
        return new Quat(W, X, Y, Z);
    }

    public Quat Inverse()
    {
        // TODO (W6):
        // For a unit quaternion:
        // inverse = (w, -x, -y, -z)
        Quat inverse = new Quat(this.w, -this.x, -this.y, -this.z);
        return inverse;
    }

    public Vector3 RotateVector(Vector3 v)
    {
        Quat p = new Quat(0, v.x, v.y, v.z);

        Quat P = this * p * this.Inverse();
        // TODO (W6):
        // 1) Convert v into a pure quaternion p
        // 2) Compute:
        //    p' = this * p * this.Inverse()
        // 3) Return the xyz part as a Vector3
        return new Vector3(P.x, P.y, P.z);
    }

    public Quaternion ToUnityQuaternion()
    {
        // TODO (W6)
        // Unity stores quaternion values as (x, y, z, w)
        // return new Quaternion(x, y, z, w);
        return new Quaternion(x, y, z, w);
    }
}