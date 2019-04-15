using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StringDriver))]
public class StringDriverEditor : DriverEditor<string,string> {


    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        base.OnInspectorGUI();
        
        
    }
}