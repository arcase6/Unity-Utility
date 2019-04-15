using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntegerDriver : Driver<int,int>
{

    [SerializeField]
    [HideInInspector]
    int offset = 0;
    public int Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    public override int GetTargetValue()
    {
        if(this.BindingSources.Count == 1)
            return BindingSources[0].getValueInteger();
        else if(this.BindingSources.Count > 1){
            int sum = 0;
            foreach(IBindingSource b in BindingSources){
                sum += b.getValueInteger();
            }          
            return sum / BindingSources.Count;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<int> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueInteger()).ToList();
    }

}
