using System.Collections;
using UnityEngine;

[System.Serializable]
public class StringEvent : UnityEngine.Events.UnityEvent<string>
{

}


public abstract class MediatorUI<T> : BindingSourceMonobehaviour, ISerializationCallbackReceiver
{

    protected bool IsInitialized = false;

    [SerializeField]
    protected T value;
    public T Value
    {
        get => value;
        set
        {
            this.value = value;
            if(IsInitialized)
                this.ShowValue();
        }
    }
    public UIComponent[] UIComponents;


    protected virtual void Start()
    {

        foreach (UIComponent ui in UIComponents)
        {
            ui.Initialize(value.ToString());
            ui.OnTextChanged.AddListener(s => this.UpdateFromText());
        }
        IsInitialized = true;
    }

    public virtual void Reset()
    {
        this.UIComponents = new UIComponent[1];
        UIComponents[0] = new UIComponent();
        foreach (System.Type UIType in UIComponent.TypeLookup.Keys)
        {
            MonoBehaviour ui = GetComponent(UIType) as MonoBehaviour;
            if (ui != null)
            {
                UIComponents[0].ComponentReference = ui;
                break;
            }
        }

        UIComponents[0].TextFormatter = GetComponent<Formatter>();
    }

    public virtual string GetUnformattedText()
    {
        if (UIComponents.Length >= 1)
        {
            return UIComponents[0].GetUnformattedText();
        }
        else
        {
            return "Unconnected";
        }
    }

    public virtual string GetDisplayText()
    {
        if (UIComponents.Length == 1)
        {
            return UIComponents[0].GetDisplayText();
        }
        else if (UIComponents.Length > 1)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            for (int i = 0; i < this.UIComponents.Length; i++)
            {
                UIComponent ui = this.UIComponents[i];
                builder.Append(ui.GetDisplayText());
                if (i != this.UIComponents.Length - 1)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }
        else
        {
            return "Unconnected";
        }
    }

    public void FocusInputField(){
        foreach(UIComponent component in this.UIComponents){
            if(component.SetFocus() == true){
                break;
            }
        }
    }


    public virtual void ShowValue()
    {
        foreach (UIComponent ui in this.UIComponents)
        {
            ui.SetDisplayText(value.ToString());
        }
    }


    //link up any text change events to this method
    //it should handle any updates
    public void UpdateFromText()
    {
        string text = GetUnformattedText();
        T valueTemp = TextToValue(text);

        if (!Value.Equals(valueTemp))
        {
            Value = valueTemp;
            NotifyChange();
        }
    }


    public abstract T TextToValue(string text);



    #region Binding Source Implementation
    
    

    public override BindingMode PrefferedMode => BindingMode.TwoWay;

    public override object getValueAsObject()
    {
        return Value;
    }

    public override string getValueString()
    {
        return this.Value.ToString();
    }

    public virtual void OnBeforeSerialize()
    {
        this.sourceType = VariableUtilities.ClassifyType(typeof(T));
    }

    public virtual void OnAfterDeserialize()
    {
    }

    public override bool LockBindingMode => true;
    #endregion
}
