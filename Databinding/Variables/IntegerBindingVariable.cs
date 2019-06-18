using UnityEngine;

[CreateAssetMenu(fileName = "IntegerBindingVariable", menuName = "Binding Sources/IntegerBindingVariable", order = 0)]
public class IntegerBindingVariable : BindingSourceScriptableObject
{
    public int StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private int value;

    public int Value { get => value; set{ this.value = value; NotifyChange();} }
    public override VariableType SourceType { get => VariableType.Integer; set{}} 
    
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
        return Value;
    }

    public override string getValueString()
    {
        return Value.ToString();
    }

    public override void setFromValueString(string valueString)
    {
        int temp = int.Parse(valueString.Split(' ')[0]);
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
        this.Value = VariableUtilities.getValueInteger(value);
    }

    #endregion
}