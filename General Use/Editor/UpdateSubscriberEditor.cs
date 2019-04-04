using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

[CustomEditor(typeof(UpdateSubscriber))]
public class UpdateSubscriberEditor : Editor
{

    ReorderableList MethodList;


    [ExecuteInEditMode]
    private void OnEnable()
    {
        if (target == null) return;

        CreateReorderableList();
        SetupReorderableListHeaderDrawer();
        SetupReorderableListElementDrawer();
    }

    private void CreateReorderableList()
    {
        MethodList = new ReorderableList(serializedObject, serializedObject.FindProperty("Methods"), true, true, true, true);
    }

    private void SetupReorderableListHeaderDrawer()
    {
        MethodList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width - 200, rect.height), "Component");
            EditorGUI.LabelField(new Rect(rect.x + rect.width - 200, rect.y, 200, rect.height), "Action");
        };
    }

    private void SetupReorderableListElementDrawer()
    {
        MethodList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = MethodList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float nameWidth = 200;
            float objectWidth = rect.width - nameWidth;
            SerializedProperty componentRef = element.FindPropertyRelative("ObjectReference");
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, objectWidth - 5, EditorGUIUtility.singleLineHeight),
                componentRef, GUIContent.none);

            string[] options;
            if (componentRef?.objectReferenceValue != null)
            {
                options = componentRef.objectReferenceValue.GetType().GetMethods()
                .Where(m => m.IsPublic && m.ReturnType == typeof(void) && !m.GetParameters().Any())
                .Select(m => m.Name).ToArray();
            }
            else { options = new string[0]; }

            SerializedProperty methodNameP = element.FindPropertyRelative("MethodName");
            int methodIndex = methodNameP.stringValue != null ? Array.IndexOf(options, methodNameP.stringValue) : 0;
            bool invalidIndex = methodIndex == -1;
            if (invalidIndex){
                 methodIndex = 0;
            }
            if (options.Length > 0){
                int newIndex = EditorGUI.Popup(
                    new Rect(rect.x + objectWidth, rect.y, nameWidth, EditorGUIUtility.singleLineHeight), methodIndex, options
                );
                if(methodIndex != newIndex || invalidIndex || methodNameP.stringValue == null)
                    methodNameP.stringValue = options[newIndex];
            }
            //EditorGUI.PropertyField(new Rect(rect.x + objectWidth, rect.y, nameWidth, EditorGUIUtility.singleLineHeight),methodNameP, GUIContent.none);
        };

    }




    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty updateEveryFrame = serializedObject.FindProperty("UpdateUsingFixedPeriod");
        EditorGUILayout.PropertyField(updateEveryFrame);
        if(updateEveryFrame.boolValue)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdatePeriod"),new GUIContent("Update Period (seconds)"));
        MethodList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

    }
}