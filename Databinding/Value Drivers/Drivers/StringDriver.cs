using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringDriver : Driver<string,string>
{
    
   

    public override string GenerateDriveValue()
    {
        if(this.SourceCount == 1)
            return BindingSources.First().getValueString();
        else if(this.SourceCount > 1){
            string appendedString = "";
            foreach(IBindingSource b in BindingSources){
                appendedString +=b.getValueString();
            }
            return appendedString;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    
}
