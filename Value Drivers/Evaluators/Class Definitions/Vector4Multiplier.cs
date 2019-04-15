using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector4Multiplier", menuName = "Driver Evaluators/Vector4/Multiplier", order = 0)]
public class Vector4Multiplier : DriverEvaluator<Vector4,Vector4>
{
    public bool MultiplyByConstant;
    public Vector4 ScaleFactor;

    public override Vector4 Evaluate(List<Vector4> sourceValues)
    {
        if(sourceValues.Count == 0)
            return new Vector4(-1,-1,-1);
        Vector4 product = Vector4.one;        
        foreach(Vector4 value in sourceValues){
            product = new Vector4(product.x * value.x,product.y * value.z, product.z * value.z);
        }
        return product;

    }

    public override Vector4 Evaluate(List<object> sourceValues)
    {
        List<Vector4> convertedValues = new List<Vector4>();
        foreach(object obj in sourceValues){
            VariableType vType = VariableUtilities.ClassifyType(obj);
            switch(vType){
                case VariableType.Vector2:
                    Vector2 v2 = (Vector2)obj;
                    convertedValues.Add(new Vector4(v2.x,v2.y,1,1));
                    break;
                case VariableType.Vector3:
                    Vector3 v3 = (Vector3)obj;
                    convertedValues.Add(new Vector4(v3.x,v3.y,v3.z,1));
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