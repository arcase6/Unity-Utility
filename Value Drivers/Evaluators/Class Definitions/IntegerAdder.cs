using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "IntegerAdder", menuName = "Driver Evaluators/Integers/Adder", order = 0)]
public class IntegerAdder : DriverEvaluator<int>
{

    public override int Evaluate(List<int> sourceValues)
    {
        return sourceValues.Sum();
    }

    //same as above, but intended to be used with non-int numeric types
    public override int Evaluate(List<object> sourceValues)
    {
        List<int> convertedValues = sourceValues.Select(s => VariableUtilities.getValueInteger(s)).ToList();
        return Evaluate(convertedValues);
    }
}