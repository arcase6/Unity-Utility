using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Linq;

[CustomEditor(typeof(BindingTunnel))]
public class BindingTunnelEditor : Editor {
    string[] AvailablePropertyNames = null;
    int selectedPropertyIndex = 0;
    [ExecuteInEditMode]
    private void OnEnable()
    {
        serializedObject.Update();
        INotifyPropertyChanged convertedSource = serializedObject.FindProperty("Source").objectReferenceValue as INotifyPropertyChanged;
        if(convertedSource != null){
            //initialize propery names
                InitializePropertyNames(convertedSource);
        }
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        SerializedProperty PropNameP = this.serializedObject.FindProperty("PropertyName");
        SerializedProperty SourceP = this.serializedObject.FindProperty("Source");
        SerializedProperty PropTypeP = this.serializedObject.FindProperty("SourceParameterType");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(SourceP);
        bool sourceChanged = EditorGUI.EndChangeCheck();


        Object unconvertedSource = SourceP.objectReferenceValue;
        INotifyPropertyChanged convertedSource = unconvertedSource as INotifyPropertyChanged;
        if(convertedSource == null && unconvertedSource != null){
                EditorGUILayout.HelpBox("The target you have selected does not implement the INotifyPropertyChanged interface. Without this reacting to changes at the source will not be possible.", MessageType.Error);    
        }
        else if(convertedSource != null){
            if(sourceChanged)
            {
                //update availabe property names
                InitializePropertyNames(convertedSource);

            }
        SerializedProperty PropertyNameP = this.serializedObject.FindProperty("PropertyName");
            EditorGUI.BeginChangeCheck();
            selectedPropertyIndex = EditorGUILayout.Popup("Property", selectedPropertyIndex, AvailablePropertyNames);
            if(EditorGUI.EndChangeCheck() || sourceChanged){
                PropNameP.stringValue = AvailablePropertyNames.ElementAtOrDefault(selectedPropertyIndex);

                System.Reflection.PropertyInfo propertyInfo = convertedSource.GetType().GetProperty(PropNameP.stringValue);
                System.Type rawType = propertyInfo.PropertyType;
                VariableType type = VariableType.Unspecified;
                if(rawType == typeof(float))
                    type = VariableType.Float;
                else if(rawType == typeof(double))
                    type = VariableType.Double;
                else if(rawType == typeof(int))
                    type = VariableType.Integer;
                else if(rawType == typeof(bool))
                    type = VariableType.Boolean;
                else if(rawType == typeof(string))
                    type = VariableType.String;



                PropTypeP.enumValueIndex = (int)type;
            }
        }

        serializedObject.ApplyModifiedProperties();
        
    }

    private void InitializePropertyNames(INotifyPropertyChanged convertedSource)
    {
        AvailablePropertyNames = convertedSource.GetType().GetProperties().Select(p => p.Name).ToArray();
        selectedPropertyIndex = 0;
        SerializedProperty PropNameP = this.serializedObject.FindProperty("PropertyName");
        string name = PropNameP?.stringValue;
        int index = System.Array.FindIndex(AvailablePropertyNames, s => s.Equals(name));
        if(index != -1) selectedPropertyIndex = index;
    }


    
}