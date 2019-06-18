using System.Collections.Generic;
using UnityEngine;

public interface IBindingSource : IObservable {

    VariableType SourceType{
        get;set;
    }
    BindingMode PrefferedMode{get;}
    
    bool LockBindingMode{get;}

    int getValueInteger();

    float getValueFloat();
    
    double getValueDouble();
    string getValueString();

    decimal getValueDecimal();

    bool getValueBoolean();

    Vector4 getValueVector4();

    Vector3 getValueVector3();

    Vector2 getValueVector2();

    Vector3Int getValueVector3Int();

    Vector2Int getValueVector2Int();

    object getValueAsObject();

    void setFromValueString(string valueString);

    void setFromObject(object value);
}
