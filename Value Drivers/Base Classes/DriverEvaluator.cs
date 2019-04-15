using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class DriverEvaluator<T,U> :  ScriptableObject{
    public abstract U Evaluate(List<T> sourceValues);

    public abstract U Evaluate(List<object> sourceValues);

    public Type GetEvaluationType()
    {
        return typeof(T);
    }
}
