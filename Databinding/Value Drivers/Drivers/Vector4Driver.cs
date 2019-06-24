using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector4Driver : Driver<Vector4,Vector4>
{

    [SerializeField]
    [HideInInspector]
    Vector4 offset = Vector4.zero;
    public Vector4 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector4 GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector4() + offset;
        else if(SourceCount > 1){
            Vector4 sum = Vector4.zero;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                Vector4 value = source.RuntimeBindingSource.getValueVector4();
                if(source.IsInverted) value *= -1;
                sum += value;
            }
            if(this.AverageSourceValues)
                sum = sum / BindingSources.Count();
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

