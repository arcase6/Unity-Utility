using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatMultiplier", menuName = "Driver Evaluators/Floats/Multiplier", order = 0)]
public class FloatMultiplier : DriverEvaluator<float>
{
    public bool MultiplyByConstant;
    public float ScaleFactor;

    public override float Evaluate(List<float> sourceValues)
    {
        if(sourceValues.Count == 0)
            return -1;
        float product = 1;
        foreach(float value in sourceValues){
            product *= value;
        }
        return product;

    }

    public override float Evaluate(List<object> sourceValues)
    {
        List<float> convertedValues = sourceValues.Select(s => VariableUtilities.getValueFloat(s)).ToList();
        return Evaluate(convertedValues);
    }
}