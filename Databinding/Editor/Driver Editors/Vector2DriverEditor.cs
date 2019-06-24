using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector2Driver))]
public class Vector2DriverEditor: DriverEditor<Vector2,Vector2> {
    
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
                ((Vector2Driver)target).SetUpdateFlag(true); 
            }
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

        
    }
}