using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BooleanBindingVariable", menuName = "Binding Sources/BooleanBindingVariable", order = 0)]
public class BooleanBindingVariable : BindingSourceScriptableObject
{
    public bool StartingValue;

    [HideInInspector]
    [SerializeField]
    private bool value;

    

    public bool Value { get => value; set { this.value = value; NotifyChange(); } }

   
    public override VariableType SourceType { get => VariableType.Boolean; set{}} 


    public override void Init() {
        //don't use property to avoid triggering an event on start
        value = StartingValue;
    }

    public override decimal getValueDecimal()
    {
        throw new System.InvalidOperationException();
    }

    public override bool getValueBoolean()
    {
        return this.value;
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
        this.Value = VariableUtilities.getValueBoolean(value);
    }

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
        return value.ToString();
    }

    public override void setFromValueString(string valueString)
    {
        if(valueString.Equals("true",StringComparison.InvariantCultureIgnoreCase)){
            Value = true;
        }
        Value = false;
    }
}