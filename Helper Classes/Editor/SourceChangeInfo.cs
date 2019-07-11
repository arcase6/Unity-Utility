using UnityEditor;

[System.Serializable]
public class SourceChangeInfo
{
    public SerializedProperty property;
    public UnityEngine.Object newSource;

    public System.Action<object> AfterMenuItemClicked;
}
