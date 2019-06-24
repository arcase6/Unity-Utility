using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BooleanDriver))]
public class BooleanDriverEditor : DriverEditor<bool,bool> {

    SerializedProperty InvertSettingP;

    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;

        InvertSettingP = serializedObject.FindProperty("invertValue");
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(InvertSettingP);
        if(EditorGUI.EndChangeCheck()){
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                ((BooleanDriver)target).SetUpdateFlag(true); 
            }
        }
        serializedObject.ApplyModifiedProperties();
        
        base.OnInspectorGUI();
        
        
    }
}