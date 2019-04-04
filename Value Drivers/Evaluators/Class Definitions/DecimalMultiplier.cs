using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DecimalMultiplier", menuName = "Driver Evaluators/Decimals/Multiplier", order = 0)]
public class DecimalMultiplier : DriverEvaluator<decimal>
{
    public bool MultiplyByConstant;
    public decimal ScaleFactor;

    public override decimal Evaluate(List<decimal> sourceValues)
    {
        if(sourceValues.Count == 0)
            return -1;
        decimal product = 1;
        foreach(decimal value in sourceValues){
            product *= value;
        }
        return product;

    }

    public override decimal Evaluate(List<object> sourceValues)
    {
        List<decimal> convertedValues = sourceValues.Select(s => VariableUtilities.getValueDecimal(s)).ToList();
        return Evaluate(convertedValues);
    }
}