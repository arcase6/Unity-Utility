using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMediator : MediatorUI<float>
{
    UIComponent targetField;

    public override float TextToValue(string text)
    {
        if(UIComponents.Length == 0){
            return -1;
        }
        else{
            return float.Parse(text);
        }
    }

    #region BindingSource Implementation
 
    public override float getValueFloat(){
        return this.Value;
    }

    public override void setFromObject(object value)
    {  
        float temp = VariableUtilities.getValueFloat(value);
        this.Value = temp;
    }

    public override void setFromValueString(string valueString)
    {
        float temp;
        if(float.TryParse(valueString.Split()[0],out temp)){
            this.Value = temp;
        }
        else{
            throw new System.NotSupportedException("value string passed could not be converted to a float: " + temp);
        }
    }

    #endregion
}
