using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntegerDriver : Driver<int,int>
{

    [SerializeField]
    [HideInInspector]
    int offset = 0;
    public int Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override int GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueInteger();
        else if(SourceCount > 1){
            int sum = 0;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                int value = source.RuntimeBindingSource.getValueInteger();
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
