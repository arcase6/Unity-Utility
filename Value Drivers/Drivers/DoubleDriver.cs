using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoubleDriver : Driver<double>
{

    [SerializeField]
    [HideInInspector]
    double offset = 0f;
    public double Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override double GetSourceValue()
    {
        if(this.BindingSources.Count == 1)
            return BindingSources[0].getValueDouble();
        else if(this.BindingSources.Count > 1)
            return BindingSources.Average(b => b.getValueDouble());
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<double> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueDouble()).ToList();
    }

    protected override double GetContextualValue()
    {
        return 0;
    }
}
