using UnityEngine;

[System.Serializable]
public class UnityPropertyData : System.IEquatable<UnityPropertyData>
{
    #region Fields
    
    [SerializeField]
    [HideInInspector]
    private Component targetComponent;
    public Component TargetComponent { get => targetComponent; }
    
    [SerializeField]
    [HideInInspector]
    private string propertyName;
    public string PropertyName { get => propertyName; set => propertyName = value; }



    [SerializeField]
    [HideInInspector]
    private bool hashDirtyBit = true;
    
    private int hashValue;

    #endregion

    #region Constructors

    //do not use this method, it exists only to work with unity's serialization system
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public UnityPropertyData(){
        this.targetComponent = null;
        this.propertyName = null;
    }

    public UnityPropertyData(Component targetComponent, string methodName){
        this.targetComponent = targetComponent;
        this.propertyName = methodName;
    }    
    #endregion

    #region MethodForTestingEquality
    public override int GetHashCode()
    {
        if(hashDirtyBit){
            if(this.propertyName == null || targetComponent == null)
                return -1;
            hashValue = TargetComponent.GetHashCode() * 7 + PropertyName.GetHashCode() * 5;
            hashDirtyBit = false;
        }
       return hashValue;
    }

    public override bool Equals(object obj){
        UnityPropertyData other = obj as UnityPropertyData;
        return this.Equals(other);
    }

    public bool Equals(UnityPropertyData other)
    {
        if(null == other){
            return false;
        }
        return this.GetHashCode() == other.GetHashCode();
    }
    #endregion
}
