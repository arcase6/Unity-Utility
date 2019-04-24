using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AngleToCircleDriver))]
public class AngleToCircleDriverEditor : DriverEditor<float, Vector3>
{
    public SerializedProperty TogglesP;
    public SerializedProperty CenterP;
    public SerializedProperty XAxisP;
    public SerializedProperty YAxisP;
    public SerializedProperty ScaleP;
    public SerializedProperty UseRadiansP;
    bool showAdvancedProperties;

    public override void OnEnable()
    {
        base.OnEnable();
        TogglesP = serializedObject.FindProperty("WriteToggles");
        CenterP = serializedObject.FindProperty("center");
        ScaleP = serializedObject.FindProperty("scale");
        XAxisP = serializedObject.FindProperty("xAxis");
        YAxisP = serializedObject.FindProperty("yAxis");
        UseRadiansP = serializedObject.FindProperty("useRadians");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        showAdvancedProperties = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvancedProperties, "Advanced Settings");
        if (showAdvancedProperties)
        {
            EditorGUILayout.PropertyField(UseRadiansP);
            EditorGUILayout.PropertyField(ScaleP);
            EditorGUILayout.PropertyField(CenterP);
            EditorGUILayout.PropertyField(XAxisP);
            EditorGUILayout.PropertyField(YAxisP);
            EditorGUILayout.PropertyField(TogglesP, new GUIContent("Write Toggle"));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }
}