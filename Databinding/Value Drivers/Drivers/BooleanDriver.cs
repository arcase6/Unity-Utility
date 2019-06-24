using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooleanDriver : Driver<bool,bool>
{

    [SerializeField]
    [HideInInspector]
    bool invertValue = false;
    public bool InvertValue{
        get{
            return invertValue;
        }
        set{
            invertValue= value;
            this.UpdateFlag = true;
        }
    }

    public override bool GenerateDriveValue()
    {
        if(SourceCount >= 1){
            return BindingSources.First().getValueBoolean() ^ InvertValue;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }
}
