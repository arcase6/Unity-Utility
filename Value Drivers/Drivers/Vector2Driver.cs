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


    public override Vector2 GetTargetValue()
    {
        if(BindingSources.Count == 1)
            return BindingSources[0].getValueVector2() + offset;
        else if(BindingSources.Count > 1){
            Vector2 sum = Vector2.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector2();
            }
            sum = sum / BindingSources.Count;
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    public override List<Vector2> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueVector2()).ToList();
    }
}

