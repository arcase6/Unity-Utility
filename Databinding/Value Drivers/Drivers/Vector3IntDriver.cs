using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector3IntDriver : Driver<Vector3Int,Vector3Int>
{

    [SerializeField]
    [HideInInspector]
    Vector3Int offset = Vector3Int.zero;
    public Vector3Int Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector3Int GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector3Int() + offset;
        else if(SourceCount > 1){
            Vector3Int sum = Vector3Int.zero;
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                Vector3Int value = source.RuntimeBindingSource.getValueVector3Int();
                if(source.IsInverted) value *= -1;
                sum += value;
            }
            if(this.AverageSourceValues)
                sum = new Vector3Int(sum.x / BindingSources.Count(),sum.y / BindingSources.Count(),sum.z / BindingSources.Count());
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

