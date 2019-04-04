using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(EventListener))]
public class EventListenerEditor : Editor {

    string[] availableComponentNames = null;

    public override void OnInspectorGUI() {
        serializedObject.Update();


        ShowSourceSelectionControls();
        //ShowBindingOptions();

        base.OnInspectorGUI();
        
    }

    private void ShowSourceSelectionControls()
    {
        SerializedProperty sourceType = serializedObject.FindProperty("SourceType");

        EditorGUI.BeginChangeCheck();

        System.Type typeToSearchFor = typeof(IObservable);
        EditorGUILayout.ObjectField(serializedObject.FindProperty("Source"), typeToSearchFor);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.FindProperty("SelectedComponentIndex").intValue = 0;
            availableComponentNames = null;
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
            }
        }
    }

    private void RegenerateComponentNames(MonoBehaviour objectReferenceValue)
    {
        availableComponentNames = objectReferenceValue.GetComponents<IBindingSource>().Select(b => b.GetType().Name).ToArray();
        for (int i = 0; i < availableComponentNames.Length; i++)
        {
            availableComponentNames[i] = availableComponentNames[i] + " (" + i + ")";
        }
    }
}