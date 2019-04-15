using UnityEngine;

[CreateAssetMenu(fileName = "Vector2IntBindingVariable", menuName = "Binding Sources/Vector2IntBindingVariable", order = 0)]
public class Vector2IntBindingVariable : BindingSourceScriptableObject
{

    public Vector2Int StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private Vector2Int value;

    public Vector2Int Value { get => value; set{ this.value = value; NotifyChange();} }
    public override VariableType SourceType { get => VariableType.Vector2Int; set{}} 
    
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
        Value = VectorExtensions.ParseVector2Int(valueString);
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
        return this.Value;
    }

    public override object getValueAsObject()
    {
        return this.Value;
    }

    public override void setFromObject(object value)
    {
        this.Value = (Vector2Int)value;
    }

    #endregion
}