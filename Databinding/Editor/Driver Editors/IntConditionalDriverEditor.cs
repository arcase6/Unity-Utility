using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntConditionalDriver))]
public class IntConditionalDriverEditor : DriverEditor<int,bool> {
    SerializedProperty OffsetP;
    SerializedProperty ConditionalEvaluatorP;

    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;

        OffsetP = serializedObject.FindProperty("offset");
        ConditionalEvaluatorP = serializedObject.FindProperty("conditionalEvaluator");
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(OffsetP);
        if(EditorGUI.EndChangeCheck()){
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                ((IntegerDriver)target).SetUpdateFlag(true); 
            }
        }
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
        
        EditorGUILayout.PropertyField(ConditionalEvaluatorP);
        serializedObject.ApplyModifiedProperties();
        
    }
}