using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloatDriver : Driver<float,float>
{

    [SerializeField]
    [HideInInspector]
    float offset = 0f;
    public float Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override float GetTargetValueStandard()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueFloat();
        else if(SourceCount > 1)
            return BindingSources.Average(b => b.getValueFloat());
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    
}
