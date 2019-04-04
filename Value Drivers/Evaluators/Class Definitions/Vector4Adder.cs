using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector4Adder", menuName = "Driver Evaluators/Vector4/Adder", order = 0)]
public class Vector4Adder : DriverEvaluator<Vector4>
{

    public override Vector4 Evaluate(List<Vector4> sourceValues)
    {
        Vector4 sum = Vector4.zero;
        foreach(Vector4 value in sourceValues){
            sum += value;
        }
        return sum;
    }

    //same as above, but intended to be used with non-Vector4 numeric types
    public override Vector4 Evaluate(List<object> sourceValues)
    {
        List<Vector4> convertedValues = new List<Vector4>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                    Vector2 v2 = (Vector2)obj;
                    convertedValues.Add(new Vector4(v2.x,v2.y,0,0));
                    break;
                case VariableType.Vector3:
                    Vector3 v3 = (Vector3)obj;
                    convertedValues.Add(new Vector4(v3.x,v3.y,v3.z,0));
                    break;
                case VariableType.Vector4:
                    convertedValues.Add((Vector4)obj);
                    break;
                default:
                    continue;
                
            }
        }

        return Evaluate(convertedValues);
    }
}