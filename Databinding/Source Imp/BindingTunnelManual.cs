using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;

public class BindingTunnelManual : BindingSourceMonobehaviour
{

    #region properties and fields
    private Delegate PropertyGetter;
    private Delegate PropertySetter;


    [SerializeField]
    private UnityEngine.Component source;
    public UnityEngine.Component Source
    {
        get
        {
            return source;
        }
    }


    [SerializeField]
    [HideInInspector]
    private string propertyName;
    public string PropertyName{
        get{
            return propertyName;
        }
    }
    
    private System.Object cachedValue = null;

    [SerializeField]
    [HideInInspector]
    private BindingUpdateMode updateMode = BindingUpdateMode.Manual;

    public BindingUpdateMode UpdateMode
    {
        get => updateMode;
        set
        {
            if (updateMode == value)
                return;
            if (updateMode == BindingUpdateMode.PropertyChangedEvent)
                UnSubscribeToPropertyChanged();
            else if (value == BindingUpdateMode.PropertyChangedEvent)
                SubscribeToPropertyChanged();
            updateMode = value;
        }
    }

    public override BindingMode PrefferedMode => BindingMode.SourceToBindingOneWay;

    public override bool LockBindingMode => false;

    #endregion

    #region binding source interface methods

    public override double getValueDouble()
    {
        if(SourceType != VariableType.Double)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return VariableUtilities.getValueDouble(cachedValue);
    }
    

    public override float getValueFloat()
    {
        if(SourceType != VariableType.Float)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return VariableUtilities.getValueFloat(cachedValue);
    }


    public override int getValueInteger()
    {
        if(SourceType != VariableType.Integer)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return VariableUtilities.getValueInteger(cachedValue);
    }

    public override bool getValueBoolean()
    {
        if(this.SourceType != VariableType.Boolean)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return VariableUtilities.getValueBoolean(cachedValue);
    }

    public override decimal getValueDecimal()
    {
        if(this.SourceType != VariableType.Decimal)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return VariableUtilities.getValueDecimal(cachedValue);
    }

    public override Vector4 getValueVector4()
    {
        if(this.SourceType != VariableType.Vector4)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return (Vector4)cachedValue;
    }

    public override Vector3 getValueVector3()
    {
        if(this.SourceType != VariableType.Vector3)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return (Vector3)cachedValue;
    }

    public override Vector2 getValueVector2()
    {
        if(this.SourceType != VariableType.Vector2)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return (Vector2)cachedValue;
    }

    public override Vector3Int getValueVector3Int()
    {
        if(this.SourceType != VariableType.Vector3Int)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return (Vector3Int)cachedValue;
    }

    public override Vector2Int getValueVector2Int()
    {
        if(this.SourceType != VariableType.Vector2Int)
            throw new System.NotSupportedException();
        cachedValue = cachedValue ?? getUpdatedValue();
        return (Vector2Int)cachedValue;
    }

    public override object getValueAsObject()
    {
        cachedValue = cachedValue ?? getUpdatedValue();
        return cachedValue;
    }
    public override string getValueString()
    {
        cachedValue = cachedValue ?? getUpdatedValue();
        return getValueString(cachedValue);
    }
    public override void setFromObject(object value)
    {
        throw new NotImplementedException();
    }

    public override void setFromValueString(string valueString)
    {
        try
        {
            switch (SourceType)
            {
                case VariableType.Integer:
                    SetSourceValue(int.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Double:
                    SetSourceValue(double.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Boolean:
                    SetSourceValue(bool.Parse(valueString));
                    break;
                case VariableType.String:
                    SetSourceValue(valueString);
                    break;
                case VariableType.Float:
                    SetSourceValue(float.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Decimal:
                    SetSourceValue(decimal.Parse(valueString.Split(' ')[0]));
                    break;
                case VariableType.Vector4:
                    SetSourceValue(VectorExtensions.ParseVector4(valueString));
                    break;
                case VariableType.Vector3:
                    SetSourceValue(VectorExtensions.ParseVector3(valueString));
                    break;
                case VariableType.Vector2:
                    SetSourceValue(VectorExtensions.ParseVector2(valueString));
                    break;
                case VariableType.Vector3Int:
                    SetSourceValue(VectorExtensions.ParseVector3Int(valueString));
                    break;
                case VariableType.Vector2Int:
                    SetSourceValue(VectorExtensions.ParseVector2Int(valueString));
                    break;
                default:
                    throw new System.NotSupportedException("The binding source does not have the variable type set or it is linked to a supported data type");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
    }

    #endregion

    #region public methods

    public void BindToNewSource(UnityEngine.Component source, string propertyName, BindingUpdateMode updateMode)
    {
        this.UpdateMode = BindingUpdateMode.Manual; // setting to manual unsubscribes from any events
        this.source = source;
        this.propertyName = propertyName;
        this.SourceType = VariableUtilities.ClassifyType(source.GetType().GetProperty(propertyName).GetValue(source));

        if (SetupPropertyDelegates() && this.UpdateMode == BindingUpdateMode.PropertyChangedEvent)
            if (source as INotifyPropertyChanged != null)
                UpdateMode = BindingUpdateMode.PropertyChangedEvent; //only set to this if it is possible to subscribe
    }

    public void PerformSourceCheck()
    {
        bool hasChanged = false;
        object currentValue = getUpdatedValue(out hasChanged);
        if (hasChanged)
        {
            cachedValue = currentValue;
            NotifyChange();
        }
    }
    public void CheckSourceChanged(object o, PropertyChangedEventArgs e)
    {
        if (this.propertyName == e.PropertyName)
        {
            PerformSourceCheck();
        }
    }



    public override string ToString()
    {
        if (Source != null && this.propertyName != null)
            return this.Source.name + ": " + this.propertyName;
        else
            return this.GetType().Name;

    }


    #endregion
    #region Unity Magic Methods

    protected virtual void OnEnable()
    {
        if (updateMode == BindingUpdateMode.PropertyChangedEvent)
            SubscribeToPropertyChanged();
    }

    private void OnDisable()
    {
        if (updateMode == BindingUpdateMode.PropertyChangedEvent)
            UnSubscribeToPropertyChanged();
    }

    private void Start()
    {
        if (PropertySetter == null)
            SetupPropertyDelegates();
    }

    #endregion

    #region Helper Methods

    private object getUpdatedValue()
    {
        bool throwaway = false;
        return getUpdatedValue(out throwaway);
    }

    private object getUpdatedValue(out bool hasChanged)
    {
        if (PropertyGetter == null)
            SetupPropertyDelegates();
        object value = null;
        switch (SourceType)
        {
            case VariableType.Double:
                double d = ((Func<double>)PropertyGetter)();
                value = d;
                break;
            case VariableType.Boolean:
                bool b = ((Func<bool>)PropertyGetter)();
                value = b;
                break;
            case VariableType.Float:
                float f = ((Func<float>)PropertyGetter)();
                value = f;
                break;
            case VariableType.Integer:
                int i = ((Func<int>)PropertyGetter)();
                value = i;
                break;
            case VariableType.Decimal:
                decimal dec = ((Func<decimal>)PropertyGetter)();
                value = dec;
                break;
            case VariableType.String:
                string s = ((Func<string>)PropertyGetter)();
                value = s;
                break;
            case VariableType.Vector4:
                Vector4 v4 = ((Func<Vector4>)PropertyGetter)();
                value = v4;
                break;
            case VariableType.Vector3:
                Vector3 v3 = ((Func<Vector3>)PropertyGetter)();
                value = v3;
                break;
            case VariableType.Vector2:
                Vector2 v2 = ((Func<Vector2>)PropertyGetter)();
                value = v2;
                break;
            case VariableType.Vector3Int:
                Vector3Int v3i = ((Func<Vector3Int>)PropertyGetter)();
                value = v3i;
                break;
            case VariableType.Vector2Int:
                Vector2Int v2i = ((Func<Vector2Int>)PropertyGetter)();
                value = v2i;
                break;
            default:
                hasChanged = false;
                return null;
        }
        hasChanged = hasValueChanged(value);

        return value;
    }

    private string getValueString(object objectRef)
    {
        switch(SourceType){
            case VariableType.Float:
                float f = VariableUtilities.getValueFloat(objectRef);
                return f.ToString();
            case VariableType.Double:
                double d = VariableUtilities.getValueDouble(objectRef);
                return d.ToString();
            case VariableType.Integer:
                int i = VariableUtilities.getValueInteger(objectRef);
                return i.ToString();
            case VariableType.Decimal:
                decimal dec = VariableUtilities.getValueDecimal(objectRef);
                return dec.ToString();
            case VariableType.Boolean:
                bool b = VariableUtilities.getValueBoolean(objectRef);
                return b.ToString();
            case VariableType.Vector4:
                Vector4 v4 = (Vector4)objectRef;
                return v4.ToString("F7");
            case VariableType.Vector3:
                Vector4 v3 = (Vector3)objectRef;
                return v3.ToString("F7");
            case VariableType.Vector2:
                Vector4 v2 = (Vector2)objectRef;
                return v2.ToString("F7");
            case VariableType.Vector3Int:
                Vector3Int v3i = (Vector3Int)objectRef;
                return v3i.ToString();
            case VariableType.Vector2Int:
                Vector2Int v2i = (Vector2Int)objectRef;
                return v2i.ToString();
            case VariableType.String:
                return objectRef.ToString();
            default:
                throw new System.NotSupportedException();
        }
    }

    private bool hasValueChanged(object currentValue)
    {
        if(cachedValue == null)
            return currentValue != null;

        try
        {
            switch (SourceType)
            {
                case VariableType.Integer:
                    return VariableUtilities.getValueInteger(cachedValue) != VariableUtilities.getValueInteger(currentValue);
                case VariableType.String:
                    return getValueString(cachedValue) != getValueString(currentValue);
                case VariableType.Double:
                    return VariableUtilities.getValueDouble(cachedValue) != VariableUtilities.getValueDouble(currentValue);
                case VariableType.Float:
                    return VariableUtilities.getValueFloat(cachedValue) != VariableUtilities.getValueFloat(currentValue);
                case VariableType.Decimal:
                    return VariableUtilities.getValueDecimal(cachedValue) != VariableUtilities.getValueDecimal(currentValue);
                case VariableType.Boolean:
                    return VariableUtilities.getValueBoolean(cachedValue) != VariableUtilities.getValueBoolean(currentValue);
                case VariableType.Vector4:
                    return ((Vector4)cachedValue) != ((Vector4)currentValue);
                case VariableType.Vector3:
                    return ((Vector3)cachedValue) != ((Vector3)currentValue);
                case VariableType.Vector2:
                    return ((Vector2)cachedValue) != ((Vector2)currentValue);
                case VariableType.Vector3Int:
                    return ((Vector3Int)cachedValue) != ((Vector3Int)currentValue);
                case VariableType.Vector2Int:
                    return ((Vector2Int)cachedValue) != ((Vector2Int)currentValue);
                default:
                    return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    private bool SetupPropertyDelegates()
    {
        if (Source == null || propertyName == null)
            return false;
        try
        {
            System.Reflection.PropertyInfo info = Source.GetType().GetProperty(propertyName);
            CreateDelegates(info);

        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to set up binding tunnel for " + (propertyName ?? "null"), Source);
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    private void CreateDelegates(System.Reflection.PropertyInfo info)
    {
        switch (SourceType)
        {
            case VariableType.Float:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<float>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<float>), source, info.GetGetMethod());
                break;
            case VariableType.Double:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<double>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<double>), source, info.GetGetMethod());
                break;
            case VariableType.String:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<string>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<string>), source, info.GetGetMethod());
                break;
            case VariableType.Boolean:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<bool>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<bool>), source, info.GetGetMethod());
                break;
            case VariableType.Integer:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<int>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<int>), source, info.GetGetMethod());
                break;
            case VariableType.Decimal:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<decimal>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<decimal>), source, info.GetGetMethod());
                break;
            case VariableType.Vector4:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<Vector4>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<Vector4>), source, info.GetGetMethod());
                break;
            case VariableType.Vector3:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<Vector3>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<Vector3>), source, info.GetGetMethod());
                break;
            case VariableType.Vector2:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<Vector2>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<Vector2>), source, info.GetGetMethod());
                break;
            case VariableType.Vector3Int:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<Vector3Int>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<Vector3Int>), source, info.GetGetMethod());
                break;
            case VariableType.Vector2Int:
                this.PropertySetter = System.Delegate.CreateDelegate(typeof(System.Action<Vector2Int>), source, info.GetSetMethod());
                this.PropertyGetter = System.Delegate.CreateDelegate(typeof(System.Func<Vector2Int>), source, info.GetGetMethod());
                break;
            default:
                throw new Exception("VariableType of specified parameter is not supported");
        }
    }

    #region source setter methods
    private void SetSourceValue(int value)
    {
        ((Action<int>)PropertySetter).Invoke(value);
    }
    private void SetSourceValue(double value)
    {
        ((Action<double>)PropertySetter).Invoke(value);
    }
    private void SetSourceValue(float value)
    {
        ((Action<float>)PropertySetter).Invoke(value);
    }
    private void SetSourceValue(string value)
    {
        ((Action<string>)PropertySetter).Invoke(value);
    }
    private void SetSourceValue(bool value)
    {
        ((Action<bool>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(decimal value){
        ((Action<decimal>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(Vector4 value){
        ((Action<Vector4>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(Vector3 value){
        ((Action<Vector3>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(Vector2 value){
        ((Action<Vector2>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(Vector3Int value){
        ((Action<Vector3Int>)PropertySetter).Invoke(value);
    }

    private void SetSourceValue(Vector2Int value){
        ((Action<Vector2Int>)PropertySetter).Invoke(value);
    }
    #endregion

    private void SubscribeToPropertyChanged()
    {
        INotifyPropertyChanged source = Source as INotifyPropertyChanged;
        if (source != null)
            source.PropertyChanged += CheckSourceChanged;
        else
            Debug.Log("You must implement the INotifyPropertyChanged interface");
    }

    private void UnSubscribeToPropertyChanged()
    {
        INotifyPropertyChanged source = Source as INotifyPropertyChanged;
        if (source != null)
            source.PropertyChanged -= CheckSourceChanged;
        else
            Debug.Log("You must implement the INotifyPropertyChanged interface");
    }

    


    #endregion
}
