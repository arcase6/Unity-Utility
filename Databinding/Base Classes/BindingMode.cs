[System.Serializable]
public enum BindingMode{
    TwoWay,
    BindingToSourceOneWay,
    SourceToBindingOneWay,
    OffsetFromSource
}

/*
    A databinding class. Changes a UIComponent when a source value changes. Does not change the source when the UIComponent changes.
    Requires UIMediator Component be attached as well (should be automatically created if not present).
 */

[System.Serializable]
public enum BindingSourceType{
    ScriptableObject,
    MonoBehaviour
}
