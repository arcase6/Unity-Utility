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

    public override decimal GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueDecimal();
        else if(SourceCount > 1){
            decimal sum = 0;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                decimal value = source.RuntimeBindingSource.getValueDecimal();
                if(source.IsInverted) value *= -1;
                sum += value;
            }
            if(this.AverageSourceValues){
                sum = sum / BindingSources.Count();
            }
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    
}
