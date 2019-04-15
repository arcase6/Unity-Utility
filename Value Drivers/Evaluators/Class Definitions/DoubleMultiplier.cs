using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleMultiplier", menuName = "Driver Evaluators/Doubles/Multiplier", order = 0)]
public class DoubleMultiplier : DriverEvaluator<double,double>
{
    public bool MultiplyByConstant;
    public double ScaleFactor;

    public override double Evaluate(List<double> sourceValues)
    {
        if(sourceValues.Count == 0)
            return -1;
        double product = 1;
        foreach(double value in sourceValues){
            product *= value;
        }
        return product;

    }

    public override double Evaluate(List<object> sourceValues)
    {
        List<double> convertedValues = sourceValues.Select(s => VariableUtilities.getValueDouble(s)).ToList();
        return Evaluate(convertedValues);
    }
}