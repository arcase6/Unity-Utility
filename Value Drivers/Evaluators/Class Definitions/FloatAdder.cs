using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatAdder", menuName = "Driver Evaluators/Floats/Adder", order = 0)]
public class FloatAdder : DriverEvaluator<float>
{

    public override float Evaluate(List<float> sourceValues)
    {
        return sourceValues.Sum();
    }

    //same as above, but intended to be used with non-float numeric types
    public override float Evaluate(List<object> sourceValues)
    {
        List<float> convertedValues = sourceValues.Select(s => VariableUtilities.getValueFloat(s)).ToList();
        return Evaluate(convertedValues);
    }
}