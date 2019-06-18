using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class MulticasterEditor<T> : Editor
{

    //maps an object to a list of property names
    //used as a cache of property names for a list of unity components
    private Dictionary<UnityEngine.Component,string[]> ComponentToPropertyLists = new Dictionary<UnityEngine.Component,string[]>(); 

    private const int ComponentWidthMin = 75;
    private const int ComponentWidthMax = 175;
    private const int PropertyWidth = 125;

    private SerializedProperty Setters;
    private SerializedProperty BindingSourcesP;
    private SerializedProperty MulticastValueP;
    
    Type allowedTargetType = typeof(T);

    ReorderableList BindingSourceList;


    #region Unity Magic Methods

    public virtual void OnEnable() {
        if(target == null) return;

        MulticastValueP = serializedObject.FindProperty("multicastValue");
        BindingSourcesP = serializedObject.FindProperty("SerializedSetters");

        CreateReorderableList();
        SetupReorderableListHeaderDrawer();
        SetupReorderableListElementDrawer();

    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //DrawTargetSelectionFields();
   
        //EditorGUILayout.ObjectField(PostProcessorP,new GUIContent(PostProcessorLabel));

        
        EditorGUILayout.PropertyField(MulticastValueP);
        BindingSourceList.DoLayoutList();

        
        

        serializedObject.ApplyModifiedProperties();
    }
    
    #endregion

      private void CreateReorderableList()
    {
        BindingSourceList = new ReorderableList(serializedObject, BindingSourcesP, true, true, true, true);
    }

    private void SetupReorderableListHeaderDrawer()
    {
        BindingSourceList.drawHeaderCallback = (Rect rect) =>
        {
            float blendFactor = Mathf.Clamp(rect.width - 150f,0,200)/ 300f;
            float ComponentWidthAdaptive = Mathf.Lerp(ComponentWidthMin,ComponentWidthMax,blendFactor);
            string ComponentLabel = ComponentWidthAdaptive > 120f ? "Target Component" : "Target Comp.";
            //float correction = 15;
            //Rect componentRect = Rect.MinMaxRect(rect.xMin,rect.yMin,rect.xMin + InvertBoxWidth + correction,rect.yMax);
            //EditorGUI.LabelField(componentRect, "Inv.");
            Rect componentRect = Rect.MinMaxRect(rect.xMin,rect.yMin,rect.xMin + ComponentWidthAdaptive,rect.yMax);
            EditorGUI.LabelField(componentRect, ComponentLabel);
            componentRect = Rect.MinMaxRect(rect.xMax - PropertyWidth,rect.yMin,rect.xMax,rect.yMax);
            EditorGUI.LabelField(componentRect, "Property Name");
        };
    }
    private void SetupReorderableListElementDrawer()
    {
        BindingSourceList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = BindingSourceList.serializedProperty.GetArrayElementAtIndex(index);

            //Rect componentRect = Rect.MinMaxRect(rect.xMin,rect.yMin,rect.xMin + InvertBoxWidth, rect.yMin + EditorGUIUtility.singleLineHeight);
            //EditorGUI.PropertyField(componentRect, element.FindPropertyRelative("IsInverted"),GUIContent.none);
            float blendFactor = Mathf.Clamp(rect.width - 150f,0,200)/ 300f;
            float ComponentWidthAdaptive = Mathf.Lerp(ComponentWidthMin,ComponentWidthMax,blendFactor);


            Rect componentRect = Rect.MinMaxRect(rect.xMin, rect.yMin, rect.xMin + ComponentWidthAdaptive, rect.yMin + EditorGUIUtility.singleLineHeight);
            SerializedProperty targetComponentP = element.FindPropertyRelative("TargetComponent");
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(componentRect, targetComponentP, GUIContent.none);
            bool refreshPropertyLists = EditorGUI.EndChangeCheck();

            //fine selection button goes here
            float fineSelectionButtonWidth = rect.width - (ComponentWidthAdaptive+ PropertyWidth);
            if(fineSelectionButtonWidth > 10){
                componentRect = Rect.MinMaxRect(componentRect.xMax, componentRect.yMin, componentRect.xMax + fineSelectionButtonWidth, componentRect.yMax);
                string componentName = "None";
                if(targetComponentP.objectReferenceValue != null)
                    componentName = targetComponentP.objectReferenceValue.ToString();
                if(GUI.Button(componentRect, componentName)){
                    System.Action<object> callback = o =>{
                        //not sure if anything needs to be done here: need to test to see if component change menu works
                    };
                    var menu = EditorHelper.CreateAvailableComponentsDropdown(targetComponentP,typeof(Component), callback);
                    menu?.ShowAsContext();
                }
            }
            //property selection button goes here

            componentRect = Rect.MinMaxRect(rect.xMax - PropertyWidth,componentRect.yMin,rect.xMax,componentRect.yMax);
            SerializedProperty propertyNameP = element.FindPropertyRelative("PropertyName");
            string[] propertyNames = getPropertyNames(targetComponentP,refreshPropertyLists);
            int selectedPropertyIndex = Array.IndexOf(propertyNames,propertyNameP.stringValue);
            if(selectedPropertyIndex == -1)
                selectedPropertyIndex = 0;
            EditorGUI.BeginChangeCheck();
            selectedPropertyIndex = EditorGUI.Popup(componentRect,selectedPropertyIndex, propertyNames);
            if(EditorGUI.EndChangeCheck()){
                if(propertyNames[0] != "None"){
                    propertyNameP.stringValue = propertyNames[selectedPropertyIndex];
                }
            }
            
            //EditorGUI.PropertyField(componentRect,, GUIContent.none);



            
             };

    }

    private string[] getPropertyNames(SerializedProperty targetComponentP,bool refreshExistingCache)
    {
        string [] propertyList = new string[]{"None"};
        Component targetComponent = targetComponentP.objectReferenceValue as Component;
        if(targetComponent == null) return propertyList;
        if (refreshExistingCache || this.ComponentToPropertyLists.TryGetValue(targetComponent, out propertyList) == false){
            //get the list of available properties, cache it, return it
            System.Reflection.PropertyInfo[] properties = targetComponent.GetType().GetProperties();
            propertyList = properties.Where(p => p.PropertyType == allowedTargetType)
            .Where(p => p.GetSetMethod() != null)
            .Select(p => p.Name).ToArray();
            this.ComponentToPropertyLists[targetComponent] = propertyList;
        }
        if(propertyList == null || !propertyList.Any())
            propertyList = new string[]{"None"};
        return propertyList;

    }


    
   
}
