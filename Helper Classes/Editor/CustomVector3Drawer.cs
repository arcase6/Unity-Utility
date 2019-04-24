using UnityEngine;
using UnityEditor;

public class CustomVector3Drawer<T>: PropertyDrawer {
    private const float labelWidth = 14;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (uses2Lines())
        {
            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }

    }

    private bool uses2Lines()
    {
        float mainLabelWidth = EditorGUIUtility.labelWidth;
        return EditorGUIUtility.currentViewWidth < mainLabelWidth + (labelWidth + GetFieldWidth()) * 3;
    }

    public virtual float GetFieldWidth(){
        return 57;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        label = EditorGUI.BeginProperty(position,label,property);
        
        bool uses2Lines = this.uses2Lines();
        if(!uses2Lines){
        position = EditorGUI.PrefixLabel(position,GUIUtility.GetControlID(FocusType.Keyboard),label);

        }
        else{
            EditorGUI.PrefixLabel(position,GUIUtility.GetControlID(FocusType.Keyboard),label);
            position = new Rect(position.x + 15, position.y + position.height /2,position.width - 15,position.height/2);
        }

        //(alternate way of correctly setting the content rectangle)
        //position = Rect.MinMaxRect(position.xMin + EditorGUIUtility.labelWidth ,position.yMin,position.xMax,position.yMax);

        float increment = position.width / 3;

        var xRect = Rect.MinMaxRect(position.xMin,position.yMin, position.x + increment, position.yMax);
        var yRect = Rect.MinMaxRect(position.xMin + increment, position.yMin,position.x + increment * 2, position.yMax);
        var zRect = Rect.MinMaxRect(position.xMin + increment * 2, position.yMin, position.xMax, position.yMax);
        
        EditorGUIUtility.labelWidth = labelWidth;

        EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"),new GUIContent("X"));
        EditorGUI.PropertyField(yRect, property.FindPropertyRelative("y"), new GUIContent("Y"));
        EditorGUI.PropertyField(zRect, property.FindPropertyRelative("z"), new GUIContent("Z"));

        EditorGUI.EndProperty();

        
    }
}