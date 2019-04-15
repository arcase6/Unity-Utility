using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3IntAdder", menuName = "Driver Evaluators/Vector3Int/Adder", order = 0)]
public class Vector3IntAdder : DriverEvaluator<Vector3Int,Vector3Int>
{

    public override Vector3Int Evaluate(List<Vector3Int> sourceValues)
    {
        Vector3Int sum = Vector3Int.zero;
        foreach(Vector3Int value in sourceValues){
            sum += value;
        }
        return sum;
    }

    //same as above, but intended to be used with non-Vector3Int numeric types
    public override Vector3Int Evaluate(List<object> sourceValues)
    {
        List<Vector3Int> convertedValues = new List<Vector3Int>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                    Vector3Int v2 = (Vector3Int)obj;
                    convertedValues.Add(new Vector3Int(v2.x,v2.y,0));
                    break;
                case VariableType.Vector3Int:
                    convertedValues.Add((Vector3Int)obj);
                    break;
                default:
                    continue;
                
            }
        }

        return Evaluate(convertedValues);
    }
}