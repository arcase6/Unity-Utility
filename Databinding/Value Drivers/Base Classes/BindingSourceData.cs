using UnityEngine;

[System.Serializable]
public class BindingSourceData{
    public IBindingSource RuntimeBindingSource;
    public Object ObjectReference;
    public BindingSourceType ReferenceType;

    public bool IsInverted;
}

