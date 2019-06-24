using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditorInternal;

public class DriverEditor<T,U> : Editor {  
    
    string[] PropertyNameOptions;
   
    private int SelectedIndex;
    private SerializedProperty BindingSourcesP;
    private SerializedProperty TargetP;
    private SerializedProperty PropertyNameP;
    private SerializedProperty PostProcessorP;
    private SerializedProperty AverageSourceValuesP;
    private SerializedProperty RunOnEnableP;
    SerializedProperty ModeOfOperationP;

    private const int TypeWidth = 110;
    private const int ObjectFieldWidth = 150;
    private const int InvertBoxWidth =20;
    ReorderableList BindingSourceList;

    System.Type TypeMonobehavior = typeof(BindingSourceMonobehaviour);
    System.Type TypeScriptableObject = typeof(BindingSourceScriptableObject);
    private System.Type allowedTargetType = typeof(U);
    private System.Type allowedSourceType = typeof(T);
    private string PostProcessorLabel;

#region Unity Magic Methods

    public virtual void OnEnable()
    {
        if (target == null) return;
        string typeName = allowedTargetType.Name.Split('.').Last();
        string sourceName = allowedSourceType.Name.Split('.').Last();
        PostProcessorLabel = "PostProcessor<" + typeName +">";
        BindingSourcesP = serializedObject.FindProperty("BindingSourcesSerializable");
        ModeOfOperationP = serializedObject.FindProperty("ModeOfOperation");
        PropertyNameP = serializedObject.FindProperty("TargetProperty");
        TargetP = serializedObject.FindProperty("DriveTarget");
        PostProcessorP = serializedObject.FindProperty("PostProcessorSerializable");
        AverageSourceValuesP = serializedObject.FindProperty("AverageSourceValues");
        RunOnEnableP = serializedObject.FindProperty("RunOnEnable");

        CreateReorderableList();
        SetupReorderableListHeaderDrawer();
        SetupReorderableListElementDrawer();
        SetupAddButton();
        
        
        RefreshTargetProperties();

    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Update")){
            ((Driver<T,U>)target).EditorUpdate();
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(RunOnEnableP);
        DrawTargetSelectionFields();
   
        EditorGUILayout.ObjectField(PostProcessorP,new GUIContent(PostProcessorLabel));

        if(BindingSourceList.count > 1){
            EditorGUILayout.PropertyField(AverageSourceValuesP);
        }

        BindingSourceList.DoLayoutList();

        
        

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
        element.FindPropertyRelative("ObjectReference").objectReferenceValue = null;

        serializedObject.ApplyModifiedProperties();
    }

#endregion
    #region binding source element drawing methods

    private void CreateReorderableList()
    {
        BindingSourceList = new ReorderableList(serializedObject, BindingSourcesP, true, true, true, true);
    }

    private void SetupReorderableListHeaderDrawer()
    {
        BindingSourceList.drawHeaderCallback = (Rect rect) =>
        {
            float correction = 15;
            Rect componentRect = Rect.MinMaxRect(rect.xMin,rect.yMin,rect.xMin + InvertBoxWidth + correction,rect.yMax);
            EditorGUI.LabelField(componentRect, "Inv.");
            componentRect = Rect.MinMaxRect(componentRect.xMax,rect.yMin,componentRect.xMax + TypeWidth,rect.yMax);
            EditorGUI.LabelField(componentRect, "Type");
            componentRect = Rect.MinMaxRect(componentRect.xMax,rect.yMin,rect.xMax,rect.yMax);
            EditorGUI.LabelField(componentRect, "IBindingSource");
        };
    }
    private void SetupReorderableListElementDrawer()
    {
        BindingSourceList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = BindingSourceList.serializedProperty.GetArrayElementAtIndex(index);

            Rect componentRect = Rect.MinMaxRect(rect.xMin,rect.yMin,rect.xMin + InvertBoxWidth, rect.yMin + EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(componentRect, element.FindPropertyRelative("IsInverted"),GUIContent.none);
            
            componentRect = Rect.MinMaxRect(componentRect.xMax,componentRect.yMin,componentRect.xMax + TypeWidth,componentRect.yMax);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(componentRect, element.FindPropertyRelative("ReferenceType"),GUIContent.none);
            if(EditorGUI.EndChangeCheck()){
                element.FindPropertyRelative("ObjectReference").objectReferenceValue = null;
            }

            bool isMonoBehavior = element.FindPropertyRelative("ReferenceType").enumValueIndex == (int)BindingSourceType.MonoBehaviour;
            System.Type searchType = isMonoBehavior ? TypeMonobehavior : TypeScriptableObject;
            SerializedProperty referencedObject = element.FindPropertyRelative("ObjectReference");
            if (!isMonoBehavior){
                componentRect = Rect.MinMaxRect(componentRect.xMax,componentRect.yMin,rect.xMax,componentRect.yMax);                
                EditorGUI.ObjectField(componentRect, referencedObject, searchType,GUIContent.none);
            }
            else
            {
                componentRect = Rect.MinMaxRect(componentRect.xMax,componentRect.yMin,componentRect.xMax + ObjectFieldWidth,componentRect.yMax); 
                bool SpaceLimited = rect.xMax - componentRect.xMax < 10;  
                if(SpaceLimited)
                    componentRect.xMax = rect.xMax;              
                EditorGUI.ObjectField(componentRect, referencedObject, searchType, new GUIContent(""));
                if (!SpaceLimited)
                {
                    Component referencedComponent = referencedObject.objectReferenceValue as Component;
                    string componentName = referencedComponent == null ? "None" : referencedComponent.ToString();
                    using (new EditorGUI.DisabledGroupScope(referencedComponent == null))
                    {
                        componentRect = Rect.MinMaxRect(componentRect.xMax,componentRect.yMin,rect.xMax,componentRect.yMax);

                        if (GUI.Button(componentRect, componentName))
                        {
                            System.Action<object> callback = o =>{
                                //not sure if anything needs to be done here: need to test to see if component change menu works                                
                            };
                            BindingSourceList.index = index;
                            var menu = EditorHelper.CreateAvailableComponentsDropdown(referencedObject,typeof(IBindingSource), callback);
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
        this.PropertyNameOptions = properties.Where(p => p.PropertyType == allowedTargetType)
        .Where(t => t.GetSetMethod() != null)        
        .Select(p => p.Name).ToArray();
        
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