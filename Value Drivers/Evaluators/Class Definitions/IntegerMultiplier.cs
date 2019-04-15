using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "IntegerMultiplier", menuName = "Driver Evaluators/Integers/Multiplier", order = 0)]
public class IntegerMultiplier : DriverEvaluator<int,int>
{
    public bool MultiplyByConstant;
    public int ScaleFactor;

    public override int Evaluate(List<int> sourceValues)
    {
        if(sourceValues.Count == 0)
            return -1;
        int product = 1;
        foreach(int value in sourceValues){
            product *= value;
        }
        return product;

    }

    public override int Evaluate(List<object> sourceValues)
    {
        List<int> convertedValues = sourceValues.Select(s => VariableUtilities.getValueInteger(s)).ToList();
        return Evaluate(convertedValues);
    }
}