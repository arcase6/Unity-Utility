using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DecimalDriver))]
public class DecimalDriverEditor : DriverEditor<decimal,decimal> {

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
                ((DecimalDriver)target).SetUpdateFlag(true); 
            }
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
        
        
    }
}