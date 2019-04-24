using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector2Driver : Driver<Vector2,Vector2>
{

    [SerializeField]
    [HideInInspector]
    Vector2 offset = Vector2.zero;
    public Vector2 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector2 GetTargetValueStandard()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector2() + offset;
        else if(SourceCount> 1){
            Vector2 sum = Vector2.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector2();
            }
            sum = sum / SourceCount;
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

