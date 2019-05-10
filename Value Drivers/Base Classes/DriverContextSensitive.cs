using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DriverContextSensitive<T, U> : Driver<T, U>
{

    public System.Func<U> GetTargetProp;

    public override bool SetupPropertyDelegates(){
        if(!base.SetupPropertyDelegates())
            return false;
        try{
            System.Reflection.PropertyInfo info = DriveTarget.GetType().GetProperty(TargetProperty);
            this.GetTargetProp = (System.Func<U>)System.Delegate.CreateDelegate(typeof(System.Func<U>), DriveTarget, info.GetGetMethod());

        }
        catch{
                Debug.Log("Failed to retrieve getter for property:" + TargetProperty + ". Make sure that the property exists and is type " + typeof(T).Name, this);
        }
        return true;
    }

    public sealed override U GenerateDriveValue(){
        return GenerateDriveValue(GetTargetProp());
    }

    protected abstract U GenerateDriveValue(U currentTargetValue);


    
}
