using UnityEngine;
public class Matrix4
{
    public Vector4 row0;
    public Vector4 row1;
    public Vector4 row2;
    public Vector4 row3;

    public Matrix4(Vector3 R, Vector3 U, Vector3 F, Vector3 P)
    {
        row0.x = R.x;
        row0.y = U.x;
        row0.z = F.x;
        row0.w = P.x;

        row1.x = R.y;
        row1.y = U.y;
        row1.z = F.y;
        row1.w = P.y;

        row2.x = R.z;
        row2.y = U.z;
        row2.z = F.z;
        row2.w = P.z;

        row3.x = 0;
        row3.y = 0;
        row3.z = 0;
        row3.w = 1;
        // TODO (W5): pack columns (R,U,F,P) into row storage:
        // row0 = (Rx, Ux, Fx, Px)
        // row1 = (Ry, Uy, Fy, Py)
        // row2 = (Rz, Uz, Fz, Pz)
        // row3 = (0,  0,  0,  1)
    }

    public Vector4 Multiply(Vector4 v)
    {
        float x = MathLib.Dot4(row0, v);
        float y = MathLib.Dot4(row1, v);
        float z = MathLib.Dot4(row2, v);
        float w = MathLib.Dot4(row3, v);

        return new Vector4(x, y, z, w);
    }
}

