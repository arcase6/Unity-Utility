using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooleanDriver : Driver<bool>
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

    public override bool GetSourceValue()
    {
        if(this.BindingSources.Count > 1){
            return BindingSources[0].getValueBoolean() ^ InvertValue;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<bool> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueBoolean()).ToList();
    }
}
