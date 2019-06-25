using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntConditionalDriver : Driver<int, bool>
{
    [SerializeField]
    [HideInInspector]
    private IntConditional conditionalEvaluator;
    public IntConditional ConditionalEvaluator
    {
        get => conditionalEvaluator;
        set
        {
            conditionalEvaluator = value;
            this.UpdateFlag = true;
        }
    }

    [SerializeField]
    [HideInInspector]
    int offset = 0;
    public int Offset
    {
        get
        {
            return offset;
        }
        set
        {
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override bool GenerateDriveValue()
    {
        return testCondition(getSourceIntegerValue());
    }


    private int getSourceIntegerValue()
    {
        if (SourceCount == 1)
            return BindingSources.First().getValueInteger();
        else if (SourceCount > 1)
        {
            int sum = 0;
            foreach (BindingSourceData source in this.BindingSourcesSerializable)
            {
                int value = source.RuntimeBindingSource.getValueInteger();
                if (source.IsInverted) value *= -1;
                sum += value;
            }
            if (this.AverageSourceValues)
            {
                sum = sum / BindingSources.Count();
            }
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
    }

    public bool testCondition(int sourceValue)
    {
        if (ConditionalEvaluator)
        {
            return ConditionalEvaluator.CheckValue(sourceValue);
        }
        return false;
    }

}
