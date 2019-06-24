using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3Driver))]
public class Vector3DriverEditor: DriverEditor<Vector3,Vector3> {
    
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
            serializedObject.ApplyModifiedProperties();
            if(EditorApplication.isPlaying || EditorApplication.isPaused){
                
                ((Vector3Driver)target).SetUpdateFlag(true); 
            }
            else if(((Vector3Driver)target).SourceCount > 0){
                ((Vector3Driver)target).EditorUpdate();  
            }
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

        
    }
}