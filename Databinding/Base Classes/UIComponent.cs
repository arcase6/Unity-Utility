using System.Collections.Generic;
using UnityEngine;
//UI component represents a link to a single UI element in Unity
//It is meant to simplify the creation of UI Mediators

[System.Serializable]
public class UIComponent
{
    public bool ContinuousUpdateMode = false;
    public Formatter TextFormatter;
    private string CachedValue;

    [SerializeField]
    private MonoBehaviour componentReference;
    public MonoBehaviour ComponentReference
    {
        get => componentReference;
        set
        {
            componentReference = value;
            System.Type sysType = componentReference.GetType();
            if (TypeLookup.ContainsKey(sysType))
                ComponentType = TypeLookup[sysType];
            else
                ComponentType = UIControlType.Invalid;
        }
    }

    //returns RawValue
    public StringEvent OnTextChanged;

    private UIControlType ComponentType;

    public static Dictionary<System.Type, UIControlType> TypeLookup;


    static UIComponent()
    {
        TypeLookup = new Dictionary<System.Type, UIControlType>();
        TypeLookup.Add(typeof(TMPro.TMP_InputField), UIControlType.TMP_InputField);
        TypeLookup.Add(typeof(TMPro.TMP_Text), UIControlType.TMP_Text);
        TypeLookup.Add(typeof(TMPro.TextMeshProUGUI), UIControlType.TMP_TextMeshProUGUI);
        TypeLookup.Add(typeof(UnityEngine.UI.InputField), UIControlType.Legacy_InputField);
        TypeLookup.Add(typeof(UnityEngine.UI.Text), UIControlType.Legacy_Text);
    }

    public void Initialize(string startingTextUnformatted)
    {
        this.ComponentReference = componentReference;
        this.SubscribeToTextChangedEvents();
        if(!startingTextUnformatted.Equals(""))
            this.SetDisplayText(startingTextUnformatted);
    }

    public string GetDisplayText()
    {
        string currentDisplayText = null;
        switch (this.ComponentType)
        {
            case UIControlType.TMP_InputField:
                currentDisplayText = ((TMPro.TMP_InputField)ComponentReference).text;
                break;
            case UIControlType.TMP_Text:
                currentDisplayText = ((TMPro.TMP_Text)ComponentReference).text;
                break;
            case UIControlType.TMP_TextMeshProUGUI:
                currentDisplayText = ((TMPro.TextMeshProUGUI)ComponentReference).text;
                break;
            case UIControlType.Legacy_InputField:
                currentDisplayText = ((UnityEngine.UI.InputField)ComponentReference).text;
                break;
            case UIControlType.Legacy_Text:
                currentDisplayText = ((UnityEngine.UI.Text)ComponentReference).text;
                break;
            default:
                Debug.Log("Invalid UI Target set at UI Mediator");
                return "";
        }
        CachePreviousVaue(currentDisplayText);
        return currentDisplayText;
    }

    public string GetUnformattedText()
    {
        if(TextFormatter)
            return TextFormatter.GetRawValue(GetDisplayText());
        else
            return GetDisplayText();
    }

    public void SetDisplayText(string text)
    {
        if (this.TextFormatter)
        {
            text = TextFormatter.GetFormattedValue(text, CachedValue);
        }
        CachePreviousVaue(text);
        switch (this.ComponentType)
        {
            case UIControlType.TMP_InputField:
                ((TMPro.TMP_InputField)ComponentReference).text = text;
                break;
            case UIControlType.TMP_Text:
                ((TMPro.TMP_Text)ComponentReference).text = text;
                break;
            case UIControlType.TMP_TextMeshProUGUI:
                ((TMPro.TextMeshProUGUI)ComponentReference).text = text;
                break;
            case UIControlType.Legacy_InputField:
                ((UnityEngine.UI.InputField)ComponentReference).text = text;
                break;
            case UIControlType.Legacy_Text:
                ((UnityEngine.UI.Text)ComponentReference).text = text;
                break;
            default:
                Debug.Log("Invalid UI Target set at UI Mediator");
                break;
        }
    }

    public bool SetFocus(){
        switch(ComponentType){
            case UIControlType.Legacy_InputField:
                ((UnityEngine.UI.InputField)ComponentReference).ActivateInputField();
                return true;

            case UIControlType.TMP_InputField:
                
                ((TMPro.TMP_InputField)ComponentReference).ActivateInputField();
                return true;
            default: 
                return false;
        }
    }

    private void SubscribeToTextChangedEvents()
    {
        switch (this.ComponentType)
        {
            case UIControlType.TMP_InputField:
                if(ContinuousUpdateMode)
                    ((TMPro.TMP_InputField)ComponentReference).onValueChanged.AddListener(RaiseSubmit);
                else{
                    ((TMPro.TMP_InputField)ComponentReference).onEndEdit.AddListener(RaiseSubmit);
                    ((TMPro.TMP_InputField)ComponentReference).onDeselect.AddListener(RaiseSubmit);
                }
                ((TMPro.TMP_InputField)ComponentReference).onSelect.AddListener(CachePreviousVaue);
                break;

            case UIControlType.Legacy_InputField:
                if(ContinuousUpdateMode)
                    ((UnityEngine.UI.InputField)ComponentReference).onEndEdit.AddListener(RaiseSubmit);
                else
                    ((UnityEngine.UI.InputField)ComponentReference).onValueChanged.AddListener(RaiseSubmit);
                break;
            default:
                break;
        }
    }

    private void RaiseSubmit(string newValue)
    {
        if (this.TextFormatter)
        {
            string formattedValue = this.TextFormatter.GetFormattedValue(newValue, CachedValue);
            if (newValue == CachedValue)
            {
                return;
            }
            this.SetDisplayText(formattedValue);
            this.OnTextChanged.Invoke(TextFormatter.GetRawValue(formattedValue));
        }
        else if (newValue != CachedValue)
        {
            this.OnTextChanged.Invoke(newValue);
        }
        else
        {

        }
    }

    private void CachePreviousVaue(string currentValue)
    {
        this.CachedValue = currentValue;
    }

    private void CachePreviousVaue(UnityEngine.EventSystems.BaseEventData eventData)
    {
        this.CachedValue = GetDisplayText();
    }


}
