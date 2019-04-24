using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3Bool : CustomVector3<bool>
{
    static readonly Vector3Bool falseVector = new Vector3Bool(false); 
    static readonly Vector3Bool trueVector = new Vector3Bool(true); 
    public static Vector3Bool FalseVector {get {return falseVector;}}
    public static Vector3Bool TrueVector {get {return trueVector;}}

    public Vector3Bool(bool x, bool y, bool z) : base(x, y, z)
    {
    }
    public Vector3Bool(bool x, bool y) : base(x, y)
    {
    }
    public Vector3Bool(bool x) : base(x)
    {
    }

    public static Vector3Bool operator|(Vector3Bool a,Vector3Bool b){
        return new Vector3Bool(a.x || b.x, a.y || b.y, a.z || b.z);
    }

    public static Vector3Bool operator&(Vector3Bool a,Vector3Bool b){
        return new Vector3Bool(a.x && b.x,a.y && b.y,a.z && b.z);
    }
    public static Vector3Bool operator!(Vector3Bool a){
        return new Vector3Bool(!a.x,!a.y,!a.z);
    }
}
