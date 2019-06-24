using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoubleDriver : Driver<double,double>
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

    public override double GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueDouble();
        else if(SourceCount > 1){
            double sum = 0;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                double value = source.RuntimeBindingSource.getValueDouble();
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
