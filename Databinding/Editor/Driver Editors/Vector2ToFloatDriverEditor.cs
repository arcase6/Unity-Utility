using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector2ToFloatDriver))]
public class Vector2ToFloatDriverEditor : DriverEditor<Vector2,float> {

    SerializedProperty OffsetP;
    SerializedProperty MaskP;

    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;

        OffsetP = serializedObject.FindProperty("offset");
        MaskP = serializedObject.FindProperty("mask");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(MaskP);
        EditorGUILayout.PropertyField(OffsetP);
        if(EditorGUI.EndChangeCheck()){
            serializedObject.ApplyModifiedProperties();
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                
                ((Vector3Driver)target).SetUpdateFlag(true); 
            }
            else if(((Vector3Driver)target).SourceCount > 0){
                ((Vector3Driver)target).EditorUpdate();  
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}