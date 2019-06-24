using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BindingSourceMonobehaviour : MonoBehaviour, IBindingSource
{
    protected List<IListener> listeners = new List<IListener>();

    [SerializeField]
    [HideInInspector]
    protected VariableType sourceType = VariableType.Unspecified;
    public VariableType SourceType { get => sourceType; set => sourceType = value; }
    public abstract BindingMode PrefferedMode { get; }
    public abstract bool LockBindingMode { get; }

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

    public virtual int getValueInteger(){
        throw new System.NotSupportedException();
    }

    public virtual float getValueFloat(){
        throw new System.NotSupportedException();
    }

    public virtual double getValueDouble(){
        throw new System.NotSupportedException();
    }

    public abstract string getValueString();

    public abstract void setFromValueString(string valueString);


    public virtual decimal getValueDecimal(){
        throw new System.NotSupportedException();
    }
    public virtual bool getValueBoolean(){
        throw new System.NotSupportedException();
    }
    public virtual Vector4 getValueVector4(){
        throw new System.NotSupportedException();
    }
    public virtual Vector3 getValueVector3(){
        throw new System.NotSupportedException();
    }
    public virtual Vector2 getValueVector2(){
        throw new System.NotSupportedException();
    }
    public virtual Vector3Int getValueVector3Int(){
        throw new System.NotSupportedException();
    }
    public virtual Vector2Int getValueVector2Int(){
        throw new System.NotSupportedException();
    }
    public abstract object getValueAsObject();
    public abstract void setFromObject(object value);
}
