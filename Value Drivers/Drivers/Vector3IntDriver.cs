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
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector3Int();
            }
            sum = new Vector3Int(sum.x/SourceCount, sum.y/SourceCount, sum.z/SourceCount);
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

