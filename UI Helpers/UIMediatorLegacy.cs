using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    The UIMediator is a component that is used to interface with a UI Element that contains text (Either TMPro or Legacy Unity Text).
    It is used when you want to do formatting on a submitted value or when you using one of the databinding components.
 */
public class UIMediatorLegacy : MonoBehaviour
{
    private UIControlType controlType;
    public UIControlType ControlType { get => controlType; }
    public UnityEventString TextChanged;

    private Formatter formatter;
    private object Control;

    private string OldTextValue;

    public bool isInitialiazed{
        get{return OldTextValue != null;}
    }


    public void Start()
    {
        if(!isInitialiazed){
        InitializeUILink();
        OldTextValue = GetText();
        formatter = GetComponent<Formatter>();
        SubscribeToTextEditEvent();
        }
    }

    private void SubscribeToTextEditEvent()
    {
        switch(ControlType){
            case UIControlType.TMP_InputField:
            ((TMPro.TMP_InputField)Control).onEndEdit.AddListener(RaiseChangedEvent);
            break;
            case UIControlType.Legacy_InputField:
            ((UnityEngine.UI.InputField)Control).onEndEdit.AddListener(RaiseChangedEvent);
            break;
            default: //other types don't have an edit event to subscribe to
            break;
        }
    }

    private void RaiseChangedEvent(string newValue){
        SetText(newValue,OldTextValue);
    }

    public void InitializeUILink()
    {
        object temp = GetComponent<TMPro.TMP_Text>();
        if (temp != null) { Control = temp; controlType = UIControlType.TMP_Text;return; }
        temp = GetComponent<TMPro.TMP_InputField>();
        if (temp != null) { Control = temp; controlType = UIControlType.TMP_InputField;return; }
        temp = GetComponent<UnityEngine.UI.Text>();
        if (temp != null) { Control = temp; controlType = UIControlType.Legacy_Text;return; }
        temp = GetComponent<UnityEngine.UI.InputField>();
        if (temp != null) { Control = temp; controlType = UIControlType.Legacy_InputField;return;}
        controlType = UIControlType.Invalid;
    }

    public void SetText(string value,string fallbackValue = null)
    {
        if(formatter != null)
            value = formatter.GetFormattedValue(value, fallbackValue);
        switch (ControlType)
        {
            case UIControlType.TMP_InputField:
                ((TMPro.TMP_InputField)Control).text = value;
                break;
            case UIControlType.TMP_Text:
                ((TMPro.TMP_Text)Control).text = value;
                break;
            case UIControlType.Legacy_InputField:
                ((UnityEngine.UI.InputField)Control).text = value;
                break;
            case UIControlType.Legacy_Text:
                ((UnityEngine.UI.Text)Control).text = value;
                break;
            default:
                return;
        }
        if (!OldTextValue.Equals(value))
            TextChanged?.Invoke(value);
        OldTextValue = value;


    }
    public string GetText()
    {
        switch (ControlType)
        {
            case UIControlType.TMP_InputField:
                return ((TMPro.TMP_InputField)Control).text;
            case UIControlType.TMP_Text:
                return ((TMPro.TMP_Text)Control).text;
            case UIControlType.Legacy_InputField:
                return ((UnityEngine.UI.InputField)Control).text;
            case UIControlType.Legacy_Text:
                return ((UnityEngine.UI.Text)Control).text;
            default:
                return "Error";
        }
    }

    public string GetTextUnformatted(){
        string controlText = GetText();
        return formatter.GetRawValue(controlText);
    }
}
