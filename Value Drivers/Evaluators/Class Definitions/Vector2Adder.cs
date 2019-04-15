using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Adder", menuName = "Driver Evaluators/Vector2/Adder", order = 0)]
public class Vector2Adder : DriverEvaluator<Vector2,Vector2>
{

    public override Vector2 Evaluate(List<Vector2> sourceValues)
    {
        Vector2 sum = Vector2.zero;
        foreach(Vector2 value in sourceValues){
            sum += value;
        }
        return sum;
    }

    //same as above, but intended to be used with non-Vector2 numeric types
    public override Vector2 Evaluate(List<object> sourceValues)
    {
        List<Vector2> convertedValues = new List<Vector2>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                case VariableType.Vector3:
                case VariableType.Vector4:
                    convertedValues.Add((Vector2)obj);
                    break;
                default:
                    continue;
                
            }
        }

        return Evaluate(convertedValues);
    }
}