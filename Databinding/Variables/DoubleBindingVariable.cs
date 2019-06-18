using UnityEngine;

[CreateAssetMenu(fileName = "DoubleBindingVariable", menuName = "Binding Sources/DoubleBindingVariable", order = 0)]
public class DoubleBindingVariable : BindingSourceScriptableObject
{
    public double StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private double value;

    public double Value { get => value; set{ this.value = value; NotifyChange();} }
    
    public override VariableType SourceType { get => VariableType.Double; set{}} 

    
    public override void Init() {
        //don't use property to avoid triggering an event on start
        value = StartingValue;
    }

    #region binding source methods

    public override double getValueDouble()
    {
        return Value;
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
        double temp = double.Parse(valueString.Split(' ')[0]);
        Value = temp;
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
        throw new System.InvalidOperationException();
    }

    public override object getValueAsObject()
    {
        return this.value;
    }

    public override void setFromObject(object value)
    {
        this.Value = VariableUtilities.getValueDouble(value);
    }

    #endregion
}