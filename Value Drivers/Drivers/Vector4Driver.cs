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


    public override Vector4 GetTargetValueStandard()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector4() + offset;
        else if(SourceCount > 1){
            Vector4 sum = Vector4.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector4();
            }
            sum = sum / SourceCount;
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

