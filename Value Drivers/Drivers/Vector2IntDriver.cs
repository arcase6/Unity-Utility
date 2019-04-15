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


    public override Vector2Int GetTargetValue()
    {
        if(BindingSources.Count == 1)
            return BindingSources[0].getValueVector2Int() + offset;
        else if(BindingSources.Count > 1){
            Vector2Int sum = Vector2Int.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector2Int();
            }
            sum = new Vector2Int(sum.x/BindingSources.Count, sum.y/BindingSources.Count);
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    public override List<Vector2Int> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueVector2Int()).ToList();
    }
}

