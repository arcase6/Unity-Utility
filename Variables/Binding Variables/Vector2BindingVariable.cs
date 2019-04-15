using UnityEngine;

[CreateAssetMenu(fileName = "Vector2BindingVariable", menuName = "Binding Sources/Vector2BindingVariable", order = 0)]
public class Vector2BindingVariable : BindingSourceScriptableObject
{

    public Vector2 StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private Vector2 value;

    public Vector2 Value { get => value; set{ this.value = value; NotifyChange();} }
    public override VariableType SourceType { get => VariableType.Vector2; set{}} 
    
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
        Value = VectorExtensions.ParseVector2(valueString);
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
    
    public override Vector2 getValueVector2()
    {
        return this.Value;
    }

    public override Vector3 getValueVector3()
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
        this.Value = (Vector2)value;
    }

    #endregion
}