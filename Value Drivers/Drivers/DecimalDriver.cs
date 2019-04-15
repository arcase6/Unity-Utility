using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecimalDriver : Driver<decimal,decimal>
{

    [SerializeField]
    [HideInInspector]
    decimal offset = 0;
    public decimal Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override decimal GetTargetValue()
    {
        if(this.BindingSources.Count == 1)
            return BindingSources[0].getValueDecimal();
        else if(this.BindingSources.Count > 1)
            return BindingSources.Average(b => b.getValueDecimal());
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<decimal> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueDecimal()).ToList();
    }
}
