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
        else if(SourceCount > 1)
            return BindingSources.Average(b => b.getValueDouble());
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    
}
