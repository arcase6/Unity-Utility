using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BindingTunnel))]
public class BindingTunnelEditor : BindingTunnelManualEditor
{

    public SerializedProperty UpdateOnIntervalP;
    public SerializedProperty TimerP;
    public SerializedProperty RefreshIntervalP;


    protected override void OnEnable()
    {
        base.OnEnable();
        this.drawEditUpdateMode = false;
        UpdateOnIntervalP = serializedObject.FindProperty("UpdateOnInterval");
        TimerP = serializedObject.FindProperty("Timer");
        RefreshIntervalP = serializedObject.FindProperty("RefreshInterval");

        serializedObject.ApplyModifiedProperties();
    }

    bool foldedOut = false;
    public override void OnInspectorGUI()
    { 
        foldedOut = EditorGUILayout.Foldout(foldedOut, "Update Interval Settings",true);
        if (foldedOut)
        {
            EditorGUILayout.PropertyField(UpdateOnIntervalP);
            if (UpdateOnIntervalP.boolValue)
            {
                EditorGUILayout.PropertyField(TimerP);
                EditorGUILayout.PropertyField(RefreshIntervalP);
            }
            serializedObject.ApplyModifiedProperties();
        }
        base.OnInspectorGUI();
        //to-do need to draw the additional update variables
    }
}