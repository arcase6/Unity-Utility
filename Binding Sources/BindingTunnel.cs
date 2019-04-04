using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class BindingTunnel : BindingSourceMonobehaviour
{
    public MonoBehaviour Source;

    public string PropertyName;

    public VariableType SourceParameterType;
    private System.Object cachedValue = null;

    private object GetUpdatedValue()
    {
        return Source.GetType().GetProperty(PropertyName).GetValue(Source);
    }

    public override double getValueDouble()
    {
        cachedValue = cachedValue ?? GetUpdatedValue();
        return getValueDouble(cachedValue);
    }
    private double getValueDouble(object objectRef)
    {
        try
        {
            switch (SourceParameterType)
            {
                case VariableType.String:
                    return -1;
                default:
                    return System.Convert.ToDouble(objectRef);
            }
        }
        catch
        {
            return -1;
        }
    }

    public override float getValueFloat()
    {
        cachedValue = cachedValue ?? GetUpdatedValue();
        return getValueFloat(cachedValue);
    }
    private float getValueFloat(object objectRef)
    {
        try
        {
            switch (SourceParameterType)
            {
                case VariableType.String:
                    return -1;
                default:
                    return System.Convert.ToSingle(objectRef);
            }
        }
        catch
        {
            return -1;
        }
    }


    public override int getValueInteger()
    {
        cachedValue = cachedValue ?? GetUpdatedValue();
        return getValueInteger(cachedValue);
    }
    private int getValueInteger(object objectRef)
    {
        try
        {
            switch (SourceParameterType)
            {
                case VariableType.String:
                    return -1;
                default:
                    return System.Convert.ToInt32(objectRef);
            }
        }
        catch
        {
            return -1;
        }
    }

    public override bool getValueBoolean()
    {
        cachedValue = cachedValue ?? GetUpdatedValue();
        return getValueBoolean(cachedValue);
    }
    private bool getValueBoolean(object objectRef)
    {
        try
        {
            switch (SourceParameterType)
            {
                case VariableType.String:
                    return false;
                default:
                    return System.Convert.ToBoolean(objectRef);
            }
        }
        catch
        {
            return false;
        }
    }

    public override string getValueString()
    {
        cachedValue = cachedValue ?? GetUpdatedValue();
        return getValueString(cachedValue);
    }
    private string getValueString(object objectRef)
    {
        try
        {
            return objectRef.ToString();
        }
        catch
        {
            return "Err";
        }
    }

    public override void setFromValueString(string valueString)
    {
        try
        {
            switch (SourceParameterType)
            {
                case VariableType.Integer:
                    SetSource(int.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Double:
                    SetSource(double.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Boolean:
                    SetSource(bool.Parse(valueString));
                    break;
                case VariableType.String:
                    SetSource(valueString);
                    break;
                default:
                case VariableType.Float:
                    SetSource(float.Parse(valueString.Split(' ')[0]));
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
    }


    #region SetSourceMethods
    private void SetSource(string valueString)
    {
        Source.GetType().GetProperty(PropertyName).SetValue(Source, valueString);
    }

    private void SetSource(bool value)
    {
        Source.GetType().GetProperty(PropertyName).SetValue(Source, value);
    }

    private void SetSource(double value)
    {
        Source.GetType().GetProperty(PropertyName).SetValue(Source, value);
    }

    private void SetSource(float value)
    {
        Source.GetType().GetProperty(PropertyName).SetValue(Source, value);
    }

    private void SetSource(int value)
    {
        Source.GetType().GetProperty(PropertyName).SetValue(Source, value);
    }

    #endregion

    private void OnEnable()
    {
        INotifyPropertyChanged source = Source as INotifyPropertyChanged;
        if (source != null)
        {
            source.PropertyChanged += CheckSourceChanged;
        }
        else
        {
            Debug.Log("You must implement the INotifyPropertyChanged interface");
        }

    }

    private void OnDisable()
    {
        INotifyPropertyChanged source = Source as INotifyPropertyChanged;
        if (source != null)
        {
            source.PropertyChanged -= CheckSourceChanged;
        }
        else
        {
            Debug.Log("You must implement the INotifyPropertyChanged interface");
        }
    }

    public void CheckSourceChanged(object o, PropertyChangedEventArgs e)
    {
        if (this.PropertyName == e.PropertyName)
        {
            object currentValue = GetUpdatedValue();
            if (this.cachedValue == null || hasValueChanged(currentValue)){
                cachedValue = currentValue;
                NotifyChange();
            }
        }
    }

    private bool hasValueChanged(object currentValue)
    {
        switch (SourceParameterType)
        {
            case VariableType.Integer:
                return getValueInteger(cachedValue) != getValueInteger(currentValue);
            case VariableType.String:
                return getValueString(cachedValue) != getValueString(currentValue);
            case VariableType.Double:
                return getValueDouble(cachedValue) != getValueDouble(currentValue);
            case VariableType.Float:
                return getValueFloat(cachedValue) != getValueFloat(currentValue);
            case VariableType.Boolean:
            default:
                return true;
        }
    }

    public override string ToString()
    {
        string name = this.Source.name + ": " + this.PropertyName;
        return name;
    }

    public override decimal getValueDecimal()
    {
        throw new System.NotImplementedException();
    }

    public override Vector4 getValueVector4()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 getValueVector3()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 getValueVector2()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3Int getValueVector3Int()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2Int getValueVector2Int()
    {
        throw new System.NotImplementedException();
    }

    public override object getValueAsObject()
    {
        throw new System.NotImplementedException();
    }

    public override void setFromObject(object value)
    {
        throw new System.NotImplementedException();
    }
}
