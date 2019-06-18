using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatToVector3Driver))]
public class FloatToVector3DriverEditor : DriverEditor<float,Vector3> {
    SerializedProperty OffsetP;
    SerializedProperty WriteTogglesP;

    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;

        OffsetP = serializedObject.FindProperty("Offset");
        WriteTogglesP = serializedObject.FindProperty("WriteToggles");
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(OffsetP);
        EditorGUILayout.PropertyField(WriteTogglesP,new GUIContent("Toggle"));
        
        if(EditorGUI.EndChangeCheck()){
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                ((DoubleDriver)target).SetUpdateFlag(true); 
            }
        }
        serializedObject.ApplyModifiedProperties();
        
        
    }
}