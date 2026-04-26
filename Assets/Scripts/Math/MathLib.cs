using UnityEngine;


public static class MathLib
{
    //my stuff
    public const double GravitationalConstant = 0.000000000066743f;
    public static float ToSingle(double value)
    {
        return (float)value;
    }
    public static double ToDouble(float value)
    {
        return (double)value;
    }
    public static float Max(float a, float b)
    {
        if ((a > b))
        {
            return (float)a;
        }
        else
        {
            return (float)b;
        }
    }

    
    //math workshops
    public static Vector2 Add(Vector2 a, Vector2 b)
    {
        Vector2 c = new Vector2(0, 0);
        c.x = a.x + b.x;
        c.y = a.y + b.y;
        return c;
    }
    public static Vector2 Subtractv2(Vector2 a, Vector2 b)
    {
        Vector2 c = new Vector2(0, 0);
        c.x = a.x - b.x;
        c.y = a.y - b.y;
        return c;
    }
    public static float Magnitude(Vector2 v)
    {
        float a = Mathf.Sqrt(v.x * v.x + v.y * v.y);
        return a;
    }
    public static float Distance(Vector2 a, Vector2 b)
    {
        float c = Magnitude(Subtract(a, b));
        return c;
    }
    public static Vector3 Add(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(0, 0, 0);
        c.x = a.x + b.x;
        c.y = a.y + b.y;
        c.z = a.z + b.z; 
        return c;
    }
    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(0, 0, 0);
        c.x = a.x - b.x;
        c.y = a.y - b.y;
        c.z = a.z - b.z; 
        return c;
    }
    public static float Magnitude(Vector3 v)
    {
        float a = Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        return a;
    }
    public static float Length(Vector3 v)
    {
        float a = Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        return a;
    }
    public static float Distance(Vector3 a, Vector3 b)
    {
        float c = Magnitude(Subtract(a, b));
        return c;
    }
    public static Vector3 Multiply(Vector3 v, float s)
    {
        Vector3 c = new Vector3(0, 0, 0);
        c.x = v.x * s;
        c.y = v.y * s;
        c.z = v.z * s;
        return c;
    }
    public static Vector3 Divide(Vector3 v, float s)
    {
        Vector3 c = new Vector3(0, 0, 0);
        if(s != 0)
        {
            c.x = v.x / s;
            c.y = v.y / s;
            c.z = v.z / s;
            return c;
        }
        else
        {
            Debug.LogError("cannot divide by 0 ");
            return Vector3.zero;
        }
    }
    public static Vector3 Normalize(Vector3 v)
    {
        Vector3 c = Divide(v, Length(v));
        return c;
    }
    public static float Dot(Vector3 a, Vector3 b)
    {
        float c = a.x * b.x + a.y * b.y + a.z * b.z;
        return c;
    }
    public static Vector3 MoveStep(Vector3 direction, float speed, float deltaTime)
    {
        Vector3 c = Multiply(Multiply(Normalize(direction), speed), deltaTime);
        return c;
    }
    public static float DegreesToRadians(float degrees)
    {
        float c = degrees * (Mathf.PI / 180);
        return c;
    }
    public static float RadiansToDegrees(float radians)
    {
        float c = radians * (180 / Mathf.PI);
        return c;
    }
    public static float AngleFromVector2(Vector2 v)
    {
        float c = Mathf.Atan2(v.y, v.x);
        return c;
    }
    public static Vector2 Vector2FromAngle(float radians)
    {
        Vector2 c = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        return c;
    }
    public static Vector3 ForwardFromYawPitch(float yawRadians, float pitchRadians)
    {
        Vector3 c = new Vector3(0, 0, 0);
        c.x = Mathf.Sin(yawRadians) * Mathf.Cos(pitchRadians);
        c.y =Mathf.Sin(pitchRadians);
        c.z = Mathf.Cos(yawRadians) * Mathf.Cos(pitchRadians);
        return c;
    }
    public static Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(0, 0, 0);
        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.x;
        return c;
    }
    public static Vector3 DirectionFromBasis(Vector3 localDir, Vector3 R, Vector3 U, Vector3 F)
    {
        Vector3 a = Multiply(R, localDir.x);
        Vector3 b = Multiply(U, localDir.y);
        Vector3 c = Multiply(F, localDir.z);
        Vector3 d = Add(a, Add(b, c));
        return d;
    }
    public static Vector3 LocalPointToWorldPoint(Vector3 P, Vector3 localPoint, Vector3 R, Vector3 U, Vector3 F)
    {
        Vector3 a = Multiply(R, P.x);
        Vector3 b = Multiply(U, P.y);
        Vector3 c = Multiply(F, P.z);
        Vector3 d = Add(P, Add(a, Add(b, c)));
        return d;
    }
    public static float Dot4(Vector4 a, Vector4 b)
    {
        float c = a.x*b.x + a.y*b.y + a.z*b.z + a.w*b.w;
        return c;
    }
    public static void BuildBasisFromForward(Vector3 forward, out Vector3 R, out Vector3 U, out Vector3 F)
    {
        F = Normalize(forward);
        U.x = 0;
        U.y = 1;
        U.z = 0;
        R = Normalize(CrossProduct(U, F));
        U = CrossProduct(F, R);
    }
    public static Vector3 TransformPoint(Matrix4 M, Vector3 p)
    {
        Vector4 v = new Vector4(p.x, p.y, p.z, 1f);
        Vector4 outV = M.Multiply(v);
        return new Vector3(outV.x, outV.y, outV.z);
    }
    public static Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float angleRad)
    {
        Normalize(axis);
        Vector3 V = v * Mathf.Cos(angleRad) + axis * Dot(v, axis) * (1 - Mathf.Cos(angleRad)) + CrossProduct(axis, v) * Mathf.Sin(angleRad);
        // TODO (W6):
        // 1) normalise axis
        // 2) calculate cos(theta) and sin(theta)
        // 3) apply Rodrigues' Rotation Formula:
        //    v' = v cos(theta)
        //       + axis * Dot(v, axis) * (1 - cos(theta))
        //       + CrossProduct(axis, v) * sin(theta)

        return V;
    }

}