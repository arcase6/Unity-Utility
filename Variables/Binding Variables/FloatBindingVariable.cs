using UnityEngine;

[CreateAssetMenu(fileName = "FloatBindingVariable", menuName = "Binding Sources/FloatBindingVariable", order = 0)]
public class FloatBindingVariable : BindingSourceScriptableObject
{

    public float StartingValue;
    
    [HideInInspector]
    [SerializeField]
    private float value;

    public float Value { get => value; set{ this.value = value; NotifyChange();} }
   
    public override VariableType SourceType { get => VariableType.Float; set{}} 

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
        return Value;
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
        float temp = float.Parse(valueString.Split(' ')[0]);
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
        this.Value = VariableUtilities.getValueFloat(value);
    }

    #endregion
}