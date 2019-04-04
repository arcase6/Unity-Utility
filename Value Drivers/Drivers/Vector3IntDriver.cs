using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector3IntDriver : Driver<Vector3Int>
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


    public override Vector3Int GetSourceValue()
    {
        if(BindingSources.Count == 1)
            return BindingSources[0].getValueVector3Int() + offset;
        else if(BindingSources.Count > 1){
            Vector3Int sum = Vector3Int.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector3Int();
            }
            sum = new Vector3Int(sum.x/BindingSources.Count, sum.y/BindingSources.Count, sum.z/BindingSources.Count);
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    public override List<Vector3Int> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueVector3Int()).ToList();
    }
}

