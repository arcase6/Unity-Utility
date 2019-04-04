using UnityEngine;

/*
    Formatters are components that convert a string value into a display value or vice versa in some cases (i.e. the display value is a number and you would like to remove units etc.)
    They are used by a UIMediator when setting text or when getting text in unformatted form.
 */
public abstract class Formatter : MonoBehaviour 
{
    public abstract string GetFormattedValue(string value, string fallbackValue = null);
    public abstract string GetRawValue(string formattedValue);
}
