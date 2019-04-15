using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditorInternal;

public class DriverEditor<T,U> : Editor {  
    
    string[] PropertyNameOptions;
   
    private int SelectedIndex;
    private SerializedProperty TargetP;
    private SerializedProperty PropertyNameP;
    private SerializedProperty EvaluatorP;
    SerializedProperty ModeOfOperationP;

    private const int TypeWidth = 110;
    private const int ObjectFieldWidth = 200;
    float PaddingHorizontal = 1;
    float PaddingVertical = 2;
    ReorderableList BindingSourceList;

    System.Type TypeMonobehavior = typeof(BindingSourceMonobehaviour);
    System.Type TypeScriptableObject = typeof(BindingSourceScriptableObject);
    private System.Type allowedTargetType = typeof(T);
    private string EvaluatorLabel;

#region Unity Magic Methods

    public virtual void OnEnable()
    {
        if (target == null) return;
        string typeName = allowedTargetType.Name.Split('.').Last();
        EvaluatorLabel = "Evaluator<" + typeName +">";
        ModeOfOperationP = serializedObject.FindProperty("ModeOfOperation");
        PropertyNameP = serializedObject.FindProperty("TargetProperty");
        TargetP = serializedObject.FindProperty("DriveTarget");
        EvaluatorP = serializedObject.FindProperty("DriverEvaluatorSerializable");
        
        CreateReorderableList();
        SetupReorderableListHeaderDrawer();
        SetupReorderableListElementDrawer();
        SetupAddButton();
        
        
        RefreshTargetProperties();

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawTargetSelectionFields();

        EditorGUILayout.PropertyField(ModeOfOperationP,new GUIContent("Evaluate Mode"));       
        EditorGUILayout.ObjectField(EvaluatorP,typeof(DriverEvaluator<T,U>),new GUIContent(EvaluatorLabel));

        BindingSourceList.DoLayoutList();

        if(GUILayout.Button("Update")){
            ((Driver<T,U>)target).EditorUpdate();
        }

        serializedObject.ApplyModifiedProperties();
    }
#endregion



#region Add New Source Functionality
    private void SetupAddButton()
    {
        BindingSourceList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("MonoBehavior"), false, AddNewBindingSource, BindingSourceType.MonoBehaviour);
            menu.AddItem(new GUIContent("Scriptable Object"), false, AddNewBindingSource, BindingSourceType.ScriptableObject);
            menu.ShowAsContext();
        };
    }

    void AddNewBindingSource(object sourceType)
    {
        BindingSourceType type = (BindingSourceType)sourceType;

        //show an object selection field based on the whether monobehavior or scriptable object
        int index = BindingSourceList.serializedProperty.arraySize;
        BindingSourceList.serializedProperty.arraySize++;
        BindingSourceList.index = index;

        SerializedProperty element = BindingSourceList.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("ReferenceType").enumValueIndex = (int)type;

        serializedObject.ApplyModifiedProperties();
    }

#endregion
    private void CreateReorderableList()
    {
        BindingSourceList = new ReorderableList(serializedObject, serializedObject.FindProperty("BindingSourcesRaw"), true, true, true, true);
    }

    private void SetupReorderableListHeaderDrawer()
    {
        BindingSourceList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(new Rect(rect.x + PaddingHorizontal * 2, rect.y, TypeWidth, rect.height), "Type");
            EditorGUI.LabelField(new Rect(rect.x + TypeWidth + PaddingHorizontal * 3, rect.y, rect.width - TypeWidth - PaddingHorizontal * 3, rect.height), "IBindingSource");
        };
    }
    #region binding source element drawing methods
    private void SetupReorderableListElementDrawer()
    {
        BindingSourceList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = BindingSourceList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(new Rect(rect.x + PaddingHorizontal, rect.y, TypeWidth, rect.y), element.FindPropertyRelative("ReferenceType"), new GUIContent(""));

            bool isMonoBehavior = element.FindPropertyRelative("ReferenceType").enumValueIndex == (int)BindingSourceType.MonoBehaviour;
            System.Type searchType = isMonoBehavior ? TypeMonobehavior : TypeScriptableObject;
            SerializedProperty referencedObject = element.FindPropertyRelative("ObjectReference");
            if (!isMonoBehavior)
                EditorGUI.ObjectField(new Rect(rect.x + PaddingHorizontal * 2 + TypeWidth, rect.y, rect.width - TypeWidth - PaddingHorizontal * 2,EditorGUIUtility.singleLineHeight), referencedObject, searchType, new GUIContent(""));
            else
            {
                float correctedObjectFieldWidth = (ObjectFieldWidth + PaddingHorizontal * 2 + TypeWidth) > rect.width ? rect.width - TypeWidth - PaddingHorizontal * 2 : ObjectFieldWidth;
                EditorGUI.ObjectField(new Rect(rect.x + PaddingHorizontal * 2 + TypeWidth, rect.y, correctedObjectFieldWidth, EditorGUIUtility.singleLineHeight), referencedObject, searchType, new GUIContent(""));
                float buttonWidth = rect.width - PaddingHorizontal * 3 - ObjectFieldWidth - TypeWidth;
                if (buttonWidth > 10 && correctedObjectFieldWidth == ObjectFieldWidth)
                {
                    Component referencedComponent = referencedObject.objectReferenceValue as Component;
                    string componentName = referencedComponent == null ? "None" : referencedComponent.ToString();
                    using (new EditorGUI.DisabledGroupScope(referencedComponent == null))
                    {
                        if (GUI.Button(new Rect(rect.x + PaddingHorizontal * 3 + ObjectFieldWidth + TypeWidth, rect.y, buttonWidth, rect.height - PaddingVertical), componentName))
                        {
                            BindingSourceList.index = index;
                            var menu = EditorHelper.CreateAvailableComponentsDropdown(referencedObject,typeof(IBindingSource));
                            menu?.ShowAsContext();
                        }
                    }
                }
            }
        };

    }
    #endregion

    

    #region target selection methods
    private void DrawTargetSelectionFields()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.ObjectField(TargetP);
        if (EditorGUI.EndChangeCheck())
        {
            RefreshTargetProperties();
        }
        if (TargetP.objectReferenceValue != null)
        {
            int index = EditorGUILayout.Popup(this.SelectedIndex, PropertyNameOptions);
            bool different = PropertyNameOptions.Length > 0 && PropertyNameP.stringValue != PropertyNameOptions[index];
            if (different)
            {
                PropertyNameP.stringValue = PropertyNameOptions[index];
                SelectedIndex = index;
            }
        }
    }

    private void RefreshTargetProperties(){
        UnityEngine.Object component = TargetP.objectReferenceValue;
        if (component == null){
            PropertyNameOptions = new string[]{"None"};
            return;
        }
        System.Reflection.PropertyInfo[] properties = component.GetType().GetProperties();
        this.PropertyNameOptions = properties.Where(p => p.PropertyType == allowedTargetType).Select(p => p.Name).ToArray();
        
        if(this.PropertyNameOptions.Length == 0)
            PropertyNameOptions = new string[]{"None"};

        if(PropertyNameP.stringValue != null){
            SelectedIndex = Array.IndexOf(PropertyNameOptions,PropertyNameP.stringValue);
            if(SelectedIndex == -1)
                SelectedIndex = 0;
        }
    }

    #endregion
}