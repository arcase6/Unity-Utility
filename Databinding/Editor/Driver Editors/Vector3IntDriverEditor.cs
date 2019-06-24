using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3IntDriver))]
public class Vector3IntDriverEditor: DriverEditor<Vector3Int,Vector3Int> {
    
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
                ((Vector3IntDriver)target).SetUpdateFlag(true); 
            }
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

        
    }
}