using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public abstract class BindingSourceScriptableObject : ScriptableObject, IBindingSource
{
    private List<IListener> listeners = new List<IListener>();

    [SerializeField]
    [HideInInspector]
    private VariableType sourceType = VariableType.Unspecified;
    public VariableType SourceType { get => sourceType; set => sourceType = value; }


    public void NotifyChange()
    {
        for(int i = listeners.Count - 1; i >= 0; i--){
            listeners[i].Notify();
        }
    }


    public void AddListener(IListener listener)
    {
        this.listeners.Add(listener);
        
    } 

    public void RemoveListener(IListener listener)
    {
        if(this.listeners.Contains(listener))
            this.listeners.Remove(listener);
    }

    public abstract int getValueInteger();

    public abstract float getValueFloat();

    public abstract double getValueDouble();

    public abstract decimal getValueDecimal();
    public abstract bool getValueBoolean();
    public abstract Vector4 getValueVector4();
    public abstract Vector3 getValueVector3();
    public abstract Vector2 getValueVector2();
    public abstract Vector3Int getValueVector3Int();
    public abstract Vector2Int getValueVector2Int();
    public abstract object getValueAsObject();
    public abstract string getValueString();
    public abstract void setFromObject(object value);

    public abstract void setFromValueString(string valueString);

    public virtual bool isModeLocked(ref BindingMode ModeToLockTo)
    {
        return false;
    }


    
    private void Awake(){
        Init();
    }

    public abstract void Init();
    
}

