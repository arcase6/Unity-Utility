using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3IntMultiplier", menuName = "Driver Evaluators/Vector3Int/Multiplier", order = 0)]
public class Vector3IntMultiplier : DriverEvaluator<Vector3Int,Vector3Int>
{
    public bool MultiplyByConstant;
    public Vector3Int ScaleFactor;

    public override Vector3Int Evaluate(List<Vector3Int> sourceValues)
    {
        if(sourceValues.Count == 0)
            return new Vector3Int(-1,-1,-1);
        Vector3Int product = Vector3Int.one;        
        foreach(Vector3Int value in sourceValues){
            product = new Vector3Int(product.x * value.x,product.y * value.z, product.z * value.z);
        }
        return product;

    }

    public override Vector3Int Evaluate(List<object> sourceValues)
    {
        List<Vector3Int> convertedValues = new List<Vector3Int>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2Int:
                    Vector2Int v2 = (Vector2Int)obj;
                    convertedValues.Add(new Vector3Int(v2.x,v2.y,1));
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