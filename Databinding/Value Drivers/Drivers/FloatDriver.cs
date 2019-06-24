using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloatDriver : Driver<float,float>
{

    [SerializeField]
    [HideInInspector]
    float offset = 0f;
    public float Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override float GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueFloat();
        else if(SourceCount > 1){
            float sum = 0f;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                float value = source.RuntimeBindingSource.getValueFloat();
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
