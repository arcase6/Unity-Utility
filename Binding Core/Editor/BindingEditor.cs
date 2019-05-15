using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Binding))]
public class BindingEditor : Editor
{
    #region cached variables

    System.Type MonoBehaviorType = typeof(BindingSourceMonobehaviour);
    System.Type ScriptableObjectType = typeof(BindingSourceScriptableObject);

    SerializedProperty SourceRawP;
    SerializedProperty SourceRawObjectP;
    SerializedProperty SourceRawTypeP;
    SerializedProperty BindingModeP;

    #endregion

    [ExecuteInEditMode]
    private void OnEnable()
    {
        SourceRawP = serializedObject.FindProperty("SourceRaw");
        SourceRawObjectP = SourceRawP.FindPropertyRelative("ObjectReference");
        SourceRawTypeP = SourceRawP.FindPropertyRelative("ReferenceType");
        BindingModeP = serializedObject.FindProperty("BindingMode");
        CheckBindingModeConstraints();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        ShowSourceSelectionControls();
        if (EditorGUI.EndChangeCheck()){
            serializedObject.ApplyModifiedProperties();
            CheckBindingModeConstraints();
        }
        ShowBindingModeDropdown();

        serializedObject.ApplyModifiedProperties();
    }

    private void CheckBindingModeConstraints()
    {
        serializedObject.Update();
        if (SourceRawObjectP.objectReferenceValue != null)
        {
            IBindingSource source = (SourceRawObjectP.objectReferenceValue as IBindingSource);
            if (source.LockBindingMode)
                BindingModeP.enumValueIndex = (int)source.PrefferedMode;
        }
    }

    private void ShowSourceSelectionControls()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Binding Type");
        EditorGUILayout.LabelField("Source Reference");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(SourceRawTypeP, new GUIContent(""));

        System.Type typeToSearchFor = SourceRawTypeP.enumValueIndex == (int)BindingSourceType.ScriptableObject ? ScriptableObjectType : MonoBehaviorType;

        EditorGUILayout.ObjectField(SourceRawObjectP, typeToSearchFor, new GUIContent(""));

        EditorGUILayout.EndHorizontal();
        if (typeToSearchFor == MonoBehaviorType)
        {
            Component referencedComponent = SourceRawObjectP.objectReferenceValue as Component;
            string componentName = referencedComponent == null ? "None" : referencedComponent.ToString();
            using (new EditorGUI.DisabledGroupScope(referencedComponent == null))
            {
                if (GUILayout.Button(componentName))
                {
                    System.Action<object> callback = o => {
                        CheckBindingModeConstraints();
                    };
                    var menu = EditorHelper.CreateAvailableComponentsDropdown(SourceRawObjectP, typeof(IBindingSource), callback);
                    menu?.ShowAsContext();
                }
            }
        }
    }

    private void ShowBindingModeDropdown()
    {
        IBindingSource source = SourceRawObjectP.objectReferenceValue as IBindingSource;

        using (new EditorGUI.DisabledGroupScope(source == null || source.LockBindingMode))
        {
            EditorGUILayout.PropertyField(BindingModeP);
        }
    }

}
