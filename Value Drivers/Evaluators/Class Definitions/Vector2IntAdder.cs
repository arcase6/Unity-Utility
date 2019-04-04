using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2IntAdder", menuName = "Driver Evaluators/Vector2Int/Adder", order = 0)]
public class Vector2IntAdder : DriverEvaluator<Vector2Int>
{

    public override Vector2Int Evaluate(List<Vector2Int> sourceValues)
    {
        Vector2Int sum = Vector2Int.zero;
        foreach(Vector2Int value in sourceValues){
            sum += value;
        }
        return sum;
    }

    //same as above, but intended to be used with non-Vector2Int numeric types
    public override Vector2Int Evaluate(List<object> sourceValues)
    {
        List<Vector2Int> convertedValues = new List<Vector2Int>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2Int:
                case VariableType.Vector3Int:
                    convertedValues.Add((Vector2Int)obj);
                    break;
                default:
                    continue;
                
            }
        }

        return Evaluate(convertedValues);
    }
}