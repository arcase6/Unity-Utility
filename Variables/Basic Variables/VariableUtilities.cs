using System.Collections.Generic;

public static class VariableUtilities{

    private static Dictionary<System.Type,VariableType> variableTypes;

    //static constructor to set up the dictionary
    static VariableUtilities(){
        variableTypes = new Dictionary<System.Type,VariableType>();
        variableTypes.Add(typeof(float),VariableType.Float);
        variableTypes.Add(typeof(double),VariableType.Double);
        variableTypes.Add(typeof(int),VariableType.Integer);
        variableTypes.Add(typeof(bool),VariableType.Boolean);
        variableTypes.Add(typeof(string),VariableType.String);
        variableTypes.Add(typeof(decimal),VariableType.Decimal);
        variableTypes.Add(typeof(UnityEngine.Vector4),VariableType.Vector4);
        variableTypes.Add(typeof(UnityEngine.Vector3),VariableType.Vector3);
        variableTypes.Add(typeof(UnityEngine.Vector2),VariableType.Vector2);
        variableTypes.Add(typeof(UnityEngine.Vector3Int),VariableType.Vector3Int);
        variableTypes.Add(typeof(UnityEngine.Vector2Int),VariableType.Vector2Int);
    }


    public static VariableType ClassifyType(object obj){
        return obj != null ? ClassifyType(obj.GetType()) :  VariableType.Unspecified;    
    }
    public static VariableType ClassifyType(System.Type rawType)
    {
        VariableType type = VariableType.Unspecified;
        if(variableTypes.TryGetValue(rawType,out type))
           return type;
        return VariableType.Unspecified;
    }

    public static int getValueInteger(object objectRef){
        return System.Convert.ToInt32(objectRef);
    }

    public static double getValueDouble(object objectRef){
        return System.Convert.ToDouble(objectRef);
    }

    public static float getValueFloat(object objectRef){
        return System.Convert.ToSingle(objectRef);
    }

    public static decimal getValueDecimal(object objectRef){
        return System.Convert.ToDecimal(objectRef);
    }

    public static bool getValueBoolean(object objectRef){
        return System.Convert.ToBoolean(objectRef);
    }
}