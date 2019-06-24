using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector3Driver : Driver<Vector3,Vector3>
{

    [SerializeField]
    [HideInInspector]
    Vector3 offset = Vector3.zero;
    public Vector3 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector3 GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector3() + offset;
        else if(SourceCount > 1){
            Vector3 sum = new Vector3(0,0,0);
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                Vector3 value = source.RuntimeBindingSource.getValueVector3();
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

