using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BooleanBindingVariable))]
public class BooleanBindingVariableEditor : Editor {
    public override void OnInspectorGUI() {
        BooleanBindingVariable variable = (BooleanBindingVariable)target;

        EditorGUI.BeginChangeCheck();
        bool newStartingValue = EditorGUILayout.Toggle("Value At Start", variable.StartingValue);
        bool newValue = EditorGUILayout.Toggle("Current Value",variable.Value);
        if(EditorGUI.EndChangeCheck()){
            if(newStartingValue != variable.StartingValue){
                Undo.RecordObject(target,"Change to variable");
                variable.StartingValue = newStartingValue;
            }
            variable.Value= newValue;

        }
    }
}