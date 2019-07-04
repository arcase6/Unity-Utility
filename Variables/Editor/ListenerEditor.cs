using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Listener))]
public class ListenerEditor : Editor {

    SerializedProperty SourceSerializedP;

    [ExecuteInEditMode]
    void OnEnable(){
        if(target){
            SourceSerializedP = serializedObject.FindProperty("SourceSerialized");
        }
    }
    public override void OnInspectorGUI() {

        serializedObject.Update();
        EditorGUILayout.PropertyField(SourceSerializedP,new GUIContent("Source<IObservable>"));
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
        
    }
}