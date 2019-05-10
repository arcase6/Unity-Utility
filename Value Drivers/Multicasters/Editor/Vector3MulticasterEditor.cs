using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3Multicaster))]
public class Vector3MulticasterEditor : MulticasterEditor<Vector3> {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
}