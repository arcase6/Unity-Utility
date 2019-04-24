using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3Double : CustomVector3<double>
{
    public Vector3Double(double x, double y, double z) : base(x, y, z)
    {
    }
    public Vector3Double(double x, double y) : base(x, y)
    {
    }

    public Vector3Double(double x) : base(x)
    {
    }
}
