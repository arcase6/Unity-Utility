using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
    public static GenericMenu CreateAvailableComponentsDropdown(SerializedProperty selectedProperty,System.Type componentType, System.Action<object> callback)
    {
        var menu = new GenericMenu();
        Component currentlySelectedComponent = selectedProperty.objectReferenceValue as Component;
        if (currentlySelectedComponent == null) return null;

        Component[] components = currentlySelectedComponent.gameObject.GetComponents(componentType);
        foreach (Component component in components)
        {
            menu.AddItem(new GUIContent(component.ToString()), false, ChangeSource, new SourceChangeInfo() { newSource = component, property = selectedProperty, AfterMenuItemClicked = callback });
        }
        return menu;
    }

    public static void ChangeSource(object changeInfo)
    {
        SourceChangeInfo changes = ((SourceChangeInfo)changeInfo);
        changes.property.objectReferenceValue = changes.newSource;
        changes.property.serializedObject.ApplyModifiedProperties();
        changes.AfterMenuItemClicked(changeInfo);
    }
}
