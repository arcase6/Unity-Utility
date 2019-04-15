using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleAdder", menuName = "Driver Evaluators/Doubles/Adder", order = 0)]
public class DoubleAdder : DriverEvaluator<double,double>
{

    public override double Evaluate(List<double> sourceValues)
    {
        return sourceValues.Sum();
    }

    //same as above, but intended to be used with non-double numeric types
    public override double Evaluate(List<object> sourceValues)
    {
        List<double> convertedValues = sourceValues.Select(s => VariableUtilities.getValueDouble(s)).ToList();
        return Evaluate(convertedValues);
    }
}