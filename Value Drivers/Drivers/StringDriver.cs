using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringDriver : Driver<string,string>
{
    

    public override string GetTargetValue()
    {
        if(this.BindingSources.Count == 1)
            return BindingSources[0].getValueString();
        else if(this.BindingSources.Count > 1){
            string appendedString = "";
            foreach(IBindingSource b in BindingSources){
                appendedString +=b.getValueString();
            }
            return appendedString;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<string> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueString()).ToList();
    }
}
