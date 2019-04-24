using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Vector3Bool))]
public class Vector3BoolDrawer: CustomVector3Drawer<bool> {
    public override float GetFieldWidth(){
        return 10;
    }
}