using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Binding))]
public class BindingEditor : Editor
{
    #region cached variables
    string[] availableComponentNames = null;
    bool twoWayRequired = false;
    BindingMode modeToLockTo;
    #endregion

    [ExecuteInEditMode]
    private void OnEnable()
    {
        serializedObject.Update();
        IBindingSource source = GetBindingSource(serializedObject.FindProperty("SourceRef").objectReferenceValue);
        if (source != null)
            twoWayRequired = source.isModeLocked(ref modeToLockTo);
        if (twoWayRequired)
            serializedObject.FindProperty("BindingMode").enumValueIndex = (int)modeToLockTo;

        serializedObject.ApplyModifiedProperties();
    }

    private IBindingSource GetBindingSource(Object sourceReference)
    {
        if (sourceReference == null)
            return null;
        IBindingSource source;
        if (serializedObject.FindProperty("SourceType").enumValueIndex == (int)BindingSourceType.MonoBehaviour)
        {
            int componentIndex = serializedObject.FindProperty("SelectedComponentIndex").intValue;
            IBindingSource[] bindingSources = ((MonoBehaviour)sourceReference).GetComponents<IBindingSource>();
            source = bindingSources.Length > componentIndex ? bindingSources[componentIndex] : null;
            if(source == null){
                serializedObject.FindProperty("SelectedComponentIndex").intValue = 0;
            }
        }
        else
        {
            source = sourceReference as IBindingSource;
        }

        return source;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShowSourceSelectionControls();
        ShowBindingOptions();
        ShowTwoWayControl();

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowSourceSelectionControls()
    {
        SerializedProperty sourceType = serializedObject.FindProperty("SourceType");

        EditorGUI.BeginChangeCheck();
        int newValue = (int)(BindingSourceType)EditorGUILayout.EnumPopup("Source Type", (BindingSourceType)sourceType.enumValueIndex);
        bool typeChanged = newValue != sourceType.enumValueIndex;
        sourceType.enumValueIndex = newValue;

        System.Type typeToSearchFor = sourceType.enumValueIndex == (int)BindingSourceType.MonoBehaviour ? typeof(BindingSourceMonobehaviour) : typeof(BindingSourceScriptableObject);
        EditorGUILayout.ObjectField(serializedObject.FindProperty("SourceRef"), typeToSearchFor);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.FindProperty("SelectedComponentIndex").intValue = 0;
            availableComponentNames = null;
            if (typeChanged)
                serializedObject.FindProperty("SourceRef").objectReferenceValue = null;
        }
    }

    private void ShowBindingOptions()
    {
        SerializedProperty sourceRef = serializedObject.FindProperty("SourceRef");
        SerializedProperty componentIndex = serializedObject.FindProperty("SelectedComponentIndex");

        MonoBehaviour sourceReference = sourceRef.objectReferenceValue as MonoBehaviour;
        if (sourceReference != null)
        {
            bool componentsChanged = availableComponentNames == null;
            if (GUILayout.Button("Refresh Source Options") || componentsChanged)
                RegenerateComponentNames(sourceReference);

            EditorGUI.BeginChangeCheck();
            componentIndex.intValue = EditorGUILayout.Popup("Source", componentIndex.intValue, availableComponentNames);
            componentsChanged = componentsChanged || EditorGUI.EndChangeCheck();

            //if the index changed from the popup or the source was changed earlier
            if (componentsChanged && componentIndex.intValue < availableComponentNames.Length)
            {
                IBindingSource source = sourceReference.GetComponents<IBindingSource>()[componentIndex.intValue];
                twoWayRequired = source.isModeLocked(ref modeToLockTo);
            }
        }
    }

    private void ShowTwoWayControl()
    {
        SerializedProperty bindingMode = serializedObject.FindProperty("BindingMode");

        if (twoWayRequired) //two way binding is not editable for some binding sources
           bindingMode.enumValueIndex = (int)BindingMode.TwoWay;
        else
            bindingMode.enumValueIndex = (int)(BindingMode)EditorGUILayout.EnumPopup("Binding　Mode", (BindingMode)bindingMode.enumValueIndex);
            
    }

    private void RegenerateComponentNames(MonoBehaviour objectReferenceValue)
    {
        availableComponentNames = objectReferenceValue.GetComponents<IBindingSource>().Select(b => b.ToString()).ToArray();
        for (int i = 0; i < availableComponentNames.Length; i++)
        {
            availableComponentNames[i] = availableComponentNames[i] + " (" + i + ")";
        }
    }

}
