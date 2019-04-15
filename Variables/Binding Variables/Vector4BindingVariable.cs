using UnityEngine;

[CreateAssetMenu(fileName = "Vector4BindingVariable", menuName = "Binding Sources/Vector4BindingVariable", order = 0)]
public class Vector4BindingVariable : BindingSourceScriptableObject
{

    public Vector4 StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private Vector4 value;

    public Vector4 Value { get => value; set{ this.value = value; NotifyChange();} }
    public override VariableType SourceType { get => VariableType.Vector4; set{}} 
    
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
        Value = VectorExtensions.ParseVector4(valueString);
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
        return this.Value;
    }

    public override Vector3 getValueVector3()
    {
        throw new System.InvalidOperationException();
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
        this.Value = (Vector4)value;
    }

    #endregion
}