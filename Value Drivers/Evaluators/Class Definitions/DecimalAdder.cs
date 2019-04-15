using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DecimalAdder", menuName = "Driver Evaluators/Decimals/Adder", order = 0)]
public class DecimalAdder : DriverEvaluator<decimal,decimal>
{

    public override decimal Evaluate(List<decimal> sourceValues)
    {
        return sourceValues.Sum();
    }

    //same as above, but intended to be used with non-decimal numeric types
    public override decimal Evaluate(List<object> sourceValues)
    {
        List<decimal> convertedValues = sourceValues.Select(s => VariableUtilities.getValueDecimal(s)).ToList();
        return Evaluate(convertedValues);
    }
}