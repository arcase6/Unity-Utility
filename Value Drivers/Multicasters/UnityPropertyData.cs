using UnityEngine;

[System.Serializable]
public struct UnityPropertyData
{
    public Component TargetComponent;
    public string PropertyName;


    public override int GetHashCode()
    {
        return TargetComponent.GetHashCode() * 3 + PropertyName.GetHashCode() * 5;
    }
}
