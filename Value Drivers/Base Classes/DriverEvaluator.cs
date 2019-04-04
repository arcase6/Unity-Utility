using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class DriverEvaluator<T> :  ScriptableObject{
    public abstract T Evaluate(List<T> sourceValues);

    public abstract T Evaluate(List<object> sourceValues);

    public Type GetEvaluationType()
    {
        return typeof(T);
    }
}
