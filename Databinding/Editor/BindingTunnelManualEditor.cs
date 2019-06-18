using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Linq;

[CustomEditor(typeof(BindingTunnelManual))]
public class BindingTunnelManualEditor : Editor
{

    public bool drawEditUpdateMode = true;
    string[] AvailablePropertyNames = null;
    int selectedPropertyIndex = 0;

    SerializedProperty PropNameP;
    SerializedProperty SourceP;
    SerializedProperty PropTypeP;
    SerializedProperty BindingUpdateModeP;

    [ExecuteInEditMode]
    protected virtual void OnEnable()
    {
        serializedObject.Update();
        drawEditUpdateMode = true;
        PropNameP = this.serializedObject.FindProperty("propertyName");
        SourceP = this.serializedObject.FindProperty("source");
        PropTypeP = this.serializedObject.FindProperty("sourceType");
        BindingUpdateModeP = this.serializedObject.FindProperty("updateMode");
        INotifyPropertyChanged convertedSource = serializedObject.FindProperty("source").objectReferenceValue as INotifyPropertyChanged;
        if (convertedSource != null)
        {
            //initialize propery names
            InitializePropertyNames(convertedSource);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(SourceP);
        UnityEngine.Component selectedSource = SourceP.objectReferenceValue as UnityEngine.Component;
        if (selectedSource == null)
            return;
        if (GUILayout.Button(selectedSource.ToString()))
        {
            System.Action<object> callback = o => {
                serializedObject.Update();
                InitializePropertyNames(SourceP.objectReferenceValue);
            };
            GenericMenu dropdownMenu = EditorHelper.CreateAvailableComponentsDropdown(SourceP, typeof(UnityEngine.Component), callback);
            dropdownMenu.ShowAsContext();
        }
        bool sourceChanged = EditorGUI.EndChangeCheck();

        
            BindingUpdateMode oldUpdateMode = (BindingUpdateMode)BindingUpdateModeP.enumValueIndex;
            BindingUpdateMode newUpdateMode = oldUpdateMode;
            if(drawEditUpdateMode)
                newUpdateMode = (BindingUpdateMode)EditorGUILayout.EnumPopup(oldUpdateMode);
            if (oldUpdateMode != newUpdateMode && drawEditUpdateMode)
                BindingUpdateModeP.enumValueIndex = (int)newUpdateMode;

            Object unconvertedSource = SourceP.objectReferenceValue;
            INotifyPropertyChanged convertedSource = unconvertedSource as INotifyPropertyChanged;
            if (convertedSource == null && unconvertedSource != null && newUpdateMode == BindingUpdateMode.PropertyChangedEvent)
            {
                EditorGUILayout.HelpBox("The target you have selected does not implement the INotifyPropertyChanged interface. Without this reacting to changes at the source will not be possible.", MessageType.Error);
            }
            else if (unconvertedSource != null)
            {
                if (sourceChanged || AvailablePropertyNames == null)
                {
                    unconvertedSource = SourceP.objectReferenceValue;
                    InitializePropertyNames(unconvertedSource);

                }
                SerializedProperty PropertyNameP = this.serializedObject.FindProperty("propertyName");
                EditorGUI.BeginChangeCheck();
                selectedPropertyIndex = EditorGUILayout.Popup("Property", selectedPropertyIndex, AvailablePropertyNames);
                if (EditorGUI.EndChangeCheck() || sourceChanged)
                {
                    PropNameP.stringValue = AvailablePropertyNames.ElementAtOrDefault(selectedPropertyIndex);
                    if (PropNameP.stringValue == null)
                    {
                        selectedPropertyIndex = 0;
                        PropNameP.stringValue = AvailablePropertyNames.ElementAtOrDefault(selectedPropertyIndex);
                    }
                    System.Reflection.PropertyInfo propertyInfo = unconvertedSource.GetType().GetProperty(PropNameP.stringValue);
                    System.Type rawType = propertyInfo.PropertyType;
                    VariableType type = VariableUtilities.ClassifyType(rawType);

                    PropTypeP.enumValueIndex = (int)type;
                }
            }

        GUI.enabled = false;
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("sourceType"));
        GUI.enabled = true;
        serializedObject.ApplyModifiedProperties();
    }




    private void InitializePropertyNames(object convertedSource)
    {
        AvailablePropertyNames = convertedSource.GetType().GetProperties()
        .Where(t => VariableUtilities.ClassifyType(t.PropertyType) != VariableType.Unspecified)
        .Where(t => t.GetGetMethod() != null)
        .Select(p => p.Name).ToArray();
        selectedPropertyIndex = 0;
        string name = PropNameP?.stringValue;
        int index = System.Array.FindIndex(AvailablePropertyNames, s => s.Equals(name));
        if (index != -1) selectedPropertyIndex = index;
    }



}