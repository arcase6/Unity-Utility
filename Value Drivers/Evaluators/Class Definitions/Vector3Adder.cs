using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Adder", menuName = "Driver Evaluators/Vector3/Adder", order = 0)]
public class Vector3Adder : DriverEvaluator<Vector3>
{

    public override Vector3 Evaluate(List<Vector3> sourceValues)
    {
        Vector3 sum = Vector3.zero;
        foreach(Vector3 value in sourceValues){
            sum += value;
        }
        return sum;
    }

    //same as above, but intended to be used with non-Vector3 numeric types
    public override Vector3 Evaluate(List<object> sourceValues)
    {
        List<Vector3> convertedValues = new List<Vector3>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                    Vector2 v2 = (Vector2)obj;
                    convertedValues.Add(new Vector3(v2.x,v2.y,0));
                    break;
                case VariableType.Vector3:
                case VariableType.Vector4:
                    convertedValues.Add((Vector3)obj);
                    break;
                default:
                    continue;
                
            }
        }

        return Evaluate(convertedValues);
    }
}