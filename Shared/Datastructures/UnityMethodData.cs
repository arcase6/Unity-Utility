using UnityEngine;

[System.Serializable]
public class UnityMethodData : System.IEquatable<UnityMethodData>
{
    #region Fields
    
    [SerializeField]
    [HideInInspector]
    private Component targetComponent;
    public Component TargetComponent { get => targetComponent; }
    
    [SerializeField]
    [HideInInspector]
    private string methodName;
    public string MethodName { get => methodName; set => methodName = value; }



    [SerializeField]
    [HideInInspector]
    private bool hashDirtyBit = true;
    
    private int hashValue;

    #endregion

    #region Constructors

    //do not use this method, it exists only to work with unity's serialization system
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public UnityMethodData(){
        this.targetComponent = null;
        this.methodName = null;
    }

    public UnityMethodData(Component targetComponent, string methodName){
        this.targetComponent = targetComponent;
        this.methodName = methodName;
    }    
    #endregion

    #region MethodForTestingEquality
    public override int GetHashCode()
    {
        if(hashDirtyBit){
            if(this.methodName == null || targetComponent == null)
                return -1;
            hashValue = TargetComponent.GetHashCode() * 7 + MethodName.GetHashCode() * 5;
            hashDirtyBit = false;
        }
       return hashValue;
    }

    public override bool Equals(object obj){
        UnityMethodData other = obj as UnityMethodData;
        return this.Equals(other);
    }

    public bool Equals(UnityMethodData other)
    {
        if(null == other){
            return false;
        }
        return this.GetHashCode() == other.GetHashCode();
    }
    #endregion
}
