using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericFormatter : Formatter
{
    

    public bool appendPrefix = false;
    public bool allowNegative = false;

    public string unitSuffix = "m";
    



    public override string GetFormattedValue(string value, string fallbackValue = null)
    {
        if(fallbackValue == null)
            fallbackValue = "0";
        double numericValue = ConvertToDouble(value, fallbackValue);

        if (!allowNegative && numericValue < 0)
            numericValue = 0;

        string formattedValue = ConvertToString(numericValue);

        return formattedValue;
    }

    public override string GetRawValue(string formattedValue){
        return ConvertToDouble(formattedValue,"-1").ToString();
    }

    private string ConvertToString(double numericValue)
    {
        string formattedValue = ((float)numericValue).ToString("#,##0.00");

        if (appendPrefix)
            formattedValue += " " + unitSuffix;
        return formattedValue;
    }

    private static double ConvertToDouble(string value, string fallbackValue)
    {
        double numericValue;
        try
        {
            if (value == "") value = fallbackValue;
            value = value.Split(' ')[0];

            if (!double.TryParse(value, out numericValue))
                numericValue = double.Parse(fallbackValue);
        }
        catch
        {
            numericValue = 0;
        }

        return numericValue;
    }
}
