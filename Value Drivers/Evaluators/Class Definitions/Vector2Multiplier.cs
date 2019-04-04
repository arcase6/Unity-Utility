using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Multiplier", menuName = "Driver Evaluators/Vector2/Multiplier", order = 0)]
public class Vector2Multiplier : DriverEvaluator<Vector2>
{
    public bool MultiplyByConstant;
    public Vector2 ScaleFactor;

    public override Vector2 Evaluate(List<Vector2> sourceValues)
    {
        if(sourceValues.Count == 0)
            return new Vector2(-1,-1);
        Vector2 product = Vector2.one;        
        foreach(Vector2 value in sourceValues){
            product *= value;
        }
        return product;

    }

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