using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//test with a float value
public class Multicaster<T> : MonoBehaviour, ISerializationCallbackReceiver
{

    //private static Dictionary<int, Action<T>> SetterCache = new Dictionary<int,Action<T>>();

    public List<UnityPropertyData> SerializedSetters = new List<UnityPropertyData>();
    private Dictionary<int, Action<T>> CachedSetterDelegates = new Dictionary<int, Action<T>>();


    [SerializeField]
    private T multicastValue;
    public T MulticastValue
    {
        get
        {
            return multicastValue;
        }
        set
        {
            multicastValue = value;
            BroadcastValue();
        }
    }

    public void BroadcastValue()
    {
        Action<T> setter;
        for (int index = SerializedSetters.Count - 1; index >= 0; index--)
        {
            try
            {
                setter = CachedSetterDelegates[SerializedSetters[index].GetHashCode()];
                setter(multicastValue);
            }
            catch
            {
                ConstructSetterAndRemoveOnFailure( index);
            }
        }
    }

    private void ConstructSetterAndRemoveOnFailure(int index)
    {
        Action<T> setter;
        bool isSetterInfoValid = false;
        try
        {
            isSetterInfoValid = InitializeSetMethod(index);
            if (isSetterInfoValid && CachedSetterDelegates.TryGetValue(SerializedSetters[index].GetHashCode(), out setter))
                if (setter != null)
                    setter(multicastValue);
        }
        catch { }
        if (!isSetterInfoValid)
        {
            SerializedSetters.RemoveAt(index); //remove to avoid wasting time on it
        }
    }

    private bool InitializeSetMethod(int index)
    {
        try
        {
            UnityPropertyData data = this.SerializedSetters[index];
            if(this.CachedSetterDelegates.ContainsKey(data.GetHashCode()))
                return true; //already created -- return early
            System.Reflection.PropertyInfo info = data.TargetComponent.GetType().GetProperty(data.PropertyName);
            Action<T> setter = (System.Action<T>)System.Delegate.CreateDelegate(typeof(System.Action<T>), data.TargetComponent, info.GetSetMethod());
            if(setter == null)
                return false;
            this.CachedSetterDelegates.Add(data.GetHashCode(), setter);
            return true;
        }
        catch
        {
            return false;
        }
    }


    public void OnAfterDeserialize()
    {
        for (int index = 0; index < CachedSetterDelegates.Count; index++)
        {
            InitializeSetMethod(index);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
