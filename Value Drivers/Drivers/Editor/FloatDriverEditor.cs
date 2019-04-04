using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatDriver))]
public class FloatDriverEditor : DriverEditor<float> {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        
    }
}