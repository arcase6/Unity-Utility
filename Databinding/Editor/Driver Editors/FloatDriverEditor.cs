using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatDriver))]
public class FloatDriverEditor : DriverEditor<float,float> {

    SerializedProperty OffsetP;

    public override void OnEnable()
    {
        base.OnEnable();
        if (target == null) return;

        OffsetP = serializedObject.FindProperty("offset");
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(OffsetP);
        if(EditorGUI.EndChangeCheck()){
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                ((FloatDriver)target).SetUpdateFlag(true); 
            }
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
        
        
    }
}