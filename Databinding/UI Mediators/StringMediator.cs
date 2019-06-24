using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringMediator : MediatorUI<string>
{
    
     protected override void Start(){
        base.Start();
        if(value.Equals(""))
            this.value = UIComponents[0].GetUnformattedText();
    }

    public override string TextToValue(string text)
    {
        return text;
    }

    #region BindingSource Implementation

    public override string getValueString(){
        return Value;
    }

    public override void setFromObject(object value)
    {  
        this.Value = value.ToString();
    }

    public override void setFromValueString(string valueString)
    {
        this.Value = valueString;
    }

    #endregion
}
