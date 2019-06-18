using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class AxisBindingSource : BindingSourceMonobehaviour
{
    public AxisBindingMode AxisBindingMode;

    public Axis AxisToUse;
    public bool InvertValue = false;

    public float Offset = 0.00f;
    float previousAxisValue;

    public override BindingMode PrefferedMode => BindingMode.SourceToBindingOneWay;

    public override bool LockBindingMode => false;

    private void Awake(){
        this.SourceType = VariableType.Float;
    }



    //this code seems to be unnecessary now since there is now on option
    //to initialize on the first frame -- probably should delete this
    private void Start() {

        previousAxisValue = getValueFloat() + 1;
        //set to some different value to trigger frame 1 update
    }

    

    public override float getValueFloat()
    {
        Vector3 position = Vector3.zero;
        if(AxisBindingMode == AxisBindingMode.WorldSpace) position = transform.position;
        else if(AxisBindingMode == AxisBindingMode.LocalSpace) position = transform.localPosition;
        else if(AxisBindingMode == AxisBindingMode.LocalSpaceIgnoreScale)position = transform.position - transform.parent.position;

        float axisValue = 0;
        
        switch (AxisToUse)
        {
            case Axis.X:
                axisValue = position.x;
                break;
            case Axis.Y:
                axisValue = position.y;
                break;
            case Axis.Z:
                axisValue = position.z;
                break;
        }
        if (InvertValue) axisValue *= -1;
        axisValue -= Offset;
        
        return axisValue;
    }

    

    public override string getValueString()
    {
        return getValueFloat().ToString("0.##");
    }

    public override object getValueAsObject()
    {
        return getValueFloat();     
    }

    public override void setFromObject(object value){
        float v = VariableUtilities.getValueFloat(value);
    }

    public override void setFromValueString(string valueString)
    {
        float value;
        try
        {
            value = float.Parse(valueString.Split(' ')[0]);
            value += Offset;
            value = InvertValue ? value * -1 : value;
        }
        catch
        {
            return;
        }

        SetValue(value);
    }

    private void SetValue(float value)
    {
        Vector3 currentPosition = Vector3.zero;
        if (AxisBindingMode == AxisBindingMode.WorldSpace) currentPosition = transform.position;
        else if (AxisBindingMode == AxisBindingMode.LocalSpace) currentPosition = transform.localPosition;
        else if (AxisBindingMode == AxisBindingMode.LocalSpaceIgnoreScale) currentPosition = transform.position - transform.parent.position;
        switch (AxisToUse)
        {
            case Axis.X:
                currentPosition.x = value;
                break;
            case Axis.Y:
                currentPosition.y = value;
                break;
            case Axis.Z:
                currentPosition.z = value;
                break;
        }
        if (AxisBindingMode == AxisBindingMode.WorldSpace)
            transform.position = currentPosition;
        else if (AxisBindingMode == AxisBindingMode.LocalSpace)
            transform.localPosition = currentPosition;
        else if (AxisBindingMode == AxisBindingMode.LocalSpaceIgnoreScale)
        {
            transform.position = transform.parent.position + currentPosition;
        }
    }


    private void LateUpdate()
    {
        if (previousAxisValue != getValueFloat())
        {
            NotifyChange();
        }
        previousAxisValue = getValueFloat();
    }

    public override string ToString(){
        string axis = this.AxisToUse == Axis.X ? "X" : "Y";
        axis = this.AxisToUse == Axis.Z ? "Z" : axis;
        string name = axis + "-Axis Binding for " + this.gameObject.name;
        return name;
    }

    #region Unsupported Getter Methods
    public override double getValueDouble()
    {
        throw new System.NotSupportedException();
    }
    public override int getValueInteger()
    {
        throw new System.NotSupportedException();
    }

    public override decimal getValueDecimal()
    {
        throw new System.NotSupportedException();
    }

    public override bool getValueBoolean()
    {
        throw new System.NotSupportedException();
    }

    public override Vector4 getValueVector4()
    {
        throw new System.NotSupportedException();
    }

    public override Vector3 getValueVector3()
    {
        throw new System.NotSupportedException();        
    }

    public override Vector2 getValueVector2()
    {
        throw new System.NotSupportedException();        
    }

    public override Vector3Int getValueVector3Int()
    {
        throw new System.NotSupportedException();
    }

    public override Vector2Int getValueVector2Int()
    {
        throw new System.NotSupportedException();
    }

    #endregion

    
}


