using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanFormatter : Formatter
{

    public string TrueDisplayValue = "True";
    public string FalseDisplayValue = "False"; 
    



    public override string GetFormattedValue(string value, string fallbackValue = null)
    {
        if(fallbackValue == null)
            fallbackValue = "false";
        bool boolValue = convertToBool(value, fallbackValue);

        return ConvertToString(boolValue);
    }

    public override string GetRawValue(string formattedValue){
        return convertToBool(formattedValue,"false").ToString();
    }

    private string ConvertToString(bool boolValue)
    {
        if(boolValue)
            return TrueDisplayValue;
        else
            return FalseDisplayValue;
    }

    private bool convertToBool(string value, string fallbackValue)
    {
        if(value.Equals(TrueDisplayValue))
            return true;
        else if(value.Equals(FalseDisplayValue))
            return false;
        else if(value.Equals("true", System.StringComparison.InvariantCultureIgnoreCase))
            return true;
        else if(fallbackValue.Equals(TrueDisplayValue))
            return true;
        else if(fallbackValue.Equals(FalseDisplayValue))
            return false;
        else if(fallbackValue.Equals("true",System.StringComparison.InvariantCultureIgnoreCase))
            return true;
        else
            return false;
    }
}
