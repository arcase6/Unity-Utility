using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2IntMultiplier", menuName = "Driver Evaluators/Vector2Int/Multiplier", order = 0)]
public class Vector2IntMultiplier : DriverEvaluator<Vector2Int, Vector2Int>
{
    public bool MultiplyByConstant;
    public Vector2Int ScaleFactor;

    public override Vector2Int Evaluate(List<Vector2Int> sourceValues)
    {
        if(sourceValues.Count == 0)
            return new Vector2Int(-1,-1);
        Vector2Int product = Vector2Int.one;        
        foreach(Vector2Int value in sourceValues){
            product *= value;
        }
        return product;

    }

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