using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class DriverEvaluator<T> :  ScriptableObject{
    public abstract T Evaluate(List<T> sourceValues, T contextValue);

    public abstract T Evaluate(List<object> sourceValues,T contextValue);

    public Type GetEvaluationType()
    {
        return typeof(T);
    }
}
