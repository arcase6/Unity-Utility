using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector2IntDriver : Driver<Vector2Int,Vector2Int>
{

    [SerializeField]
    [HideInInspector]
    Vector2Int offset = Vector2Int.zero;
    public Vector2Int Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector2Int GetTargetValueStandard()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector2Int() + offset;
        else if(SourceCount > 1){
            Vector2Int sum = Vector2Int.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector2Int();
            }
            sum = new Vector2Int(sum.x/SourceCount, sum.y/SourceCount);
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

