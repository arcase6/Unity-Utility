using UnityEngine;

[CreateAssetMenu(fileName = "Vector3BindingVariable", menuName = "Binding Sources/Vector3BindingVariable", order = 0)]
public class Vector3BindingVariable : BindingSourceScriptableObject
{

    public Vector3 StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private Vector3 value;

    public Vector3 Value { get => value; set{ this.value = value; NotifyChange();} }
    public override VariableType SourceType { get => VariableType.Vector3; set{}} 
    
    public override void Init() {
        //don't use property to avoid triggering an event on start
        value = StartingValue;
    }

    #region binding source methods

    public override double getValueDouble()
    {
        throw new System.InvalidOperationException();        
    }

    public override float getValueFloat()
    {
        throw new System.InvalidOperationException();
    }

    public override int getValueInteger()
    {
        throw new System.InvalidOperationException();
    }

    public override string getValueString()
    {
        return Value.ToString();
    }

    public override void setFromValueString(string valueString)
    {
        Value = VectorExtensions.ParseVector3(valueString);
    }

    public override decimal getValueDecimal()
    {
        throw new System.InvalidOperationException();
    }

    public override bool getValueBoolean()
    {
        throw new System.InvalidOperationException();
    }
    public override Vector4 getValueVector4()
    {
        throw new System.InvalidOperationException();
    }
    public override Vector3 getValueVector3()
    {
        return this.Value;
    }


    public override Vector2 getValueVector2()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector3Int getValueVector3Int()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector2Int getValueVector2Int()
    {
        throw new System.InvalidOperationException();
    }

    public override object getValueAsObject()
    {
        return this.Value;
    }

    public override void setFromObject(object value)
    {
        this.Value = (Vector3)value;
    }

    #endregion
}