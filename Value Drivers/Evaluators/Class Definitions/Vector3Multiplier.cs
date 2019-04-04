using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Multiplier", menuName = "Driver Evaluators/Vector3/Multiplier", order = 0)]
public class Vector3Multiplier : DriverEvaluator<Vector3>
{
    public bool MultiplyByConstant;
    public Vector3 ScaleFactor;

    public override Vector3 Evaluate(List<Vector3> sourceValues)
    {
        if(sourceValues.Count == 0)
            return new Vector3(-1,-1,-1);
        Vector3 product = Vector3.one;        
        foreach(Vector3 value in sourceValues){
            product = new Vector3(product.x * value.x,product.y * value.z, product.z * value.z);
        }
        return product;

    }

    public override Vector3 Evaluate(List<object> sourceValues)
    {
        List<Vector3> convertedValues = new List<Vector3>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                    Vector2 v2 = (Vector2)obj;
                    convertedValues.Add(new Vector3(v2.x,v2.y,1));
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