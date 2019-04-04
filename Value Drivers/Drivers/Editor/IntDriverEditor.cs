using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntegerDriver))]
public class IntegerDriverEditor : DriverEditor<int> {

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
                ((IntegerDriver)target).SetUpdateFlag(true); 
            }
        }

        base.OnInspectorGUI();
        
        
    }
}