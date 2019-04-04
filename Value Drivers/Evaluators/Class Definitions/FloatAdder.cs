using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatAdder", menuName = "Drivers/FloatAdder", order = 0)]
public class FloatAdder : DriverEvaluator<float>
{

    public override float Evaluate(List<float> sourceValues, float contextValue)
    {
        return sourceValues.Sum();
    }

    public override float Evaluate(List<object> sourceValues, float contextValue)
    {
        throw new System.NotSupportedException();
    }
}