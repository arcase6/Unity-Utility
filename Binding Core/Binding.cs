using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Binding : MonoBehaviour, IListener
{
    public bool InitializeFromSource = false;
    //public bool IsTwoWay = false;
    [HideInInspector]
    public BindingMode BindingMode;

    [HideInInspector]
    public BindingSourceType SourceType = BindingSourceType.MonoBehaviour;

    [HideInInspector]
    public int SelectedComponentIndex;
    public IBindingSource Source
    {
        set
        {
            if (value as Object != null)
            {
                source = value;
                SourceRef = value as Object;
            }
        }
        get => source;
    }

    [HideInInspector]
    [SerializeField]
    private Object SourceRef; //included so unity can serialize properly
    private IBindingSource source;
    private float cachedSourceValue = 0.0f;
    private UIMediator Mediator;

    private void Reset()
    {
        if (GetComponent<UIMediator>() == null)
        {
            gameObject.AddComponent(typeof(UIMediator));
        }
    }

    private void Awake()
    {
        if (SourceType == BindingSourceType.ScriptableObject)
            source = (IBindingSource)SourceRef;
        else
            source = ((MonoBehaviour)SourceRef).GetComponents<IBindingSource>()[SelectedComponentIndex];
        BindingMode temp = BindingMode.SourceToBindingOneWay;
        if(source.isModeLocked(ref temp))
            this.BindingMode = temp;

        Mediator = GetComponent<UIMediator>();
    }

    private void OnEnable()
    {
        cachedSourceValue = source.getValueFloat();
        source.AddListener(this);
        if(!Mediator.isInitialiazed)
            Mediator.Start();
        Mediator.TextChanged.AddListener(TextChanged);
        if (InitializeFromSource)
            SyncSource();
    }

    private void OnDisable()
    {
        source.RemoveListener(this);
        Mediator.TextChanged.RemoveListener(TextChanged);
    }

    public virtual void Notify()
    {
        if (BindingMode == BindingMode.BindingToSourceOneWay)
        {
            string sourceValue = this.source.getValueString();
            string bindingValue = this.Mediator.GetText();
            if (sourceValue != bindingValue)
                source.setFromValueString(bindingValue);
        }
        else if(BindingMode == BindingMode.OffsetFromSource){
            float sourceValue = source.getValueFloat();
            float difference = sourceValue - cachedSourceValue;
            if(Mathf.Abs(difference) >= .01f){
                float displayValue = float.Parse(this.Mediator.GetTextUnformatted());
                displayValue += difference;
                cachedSourceValue = sourceValue;
                Mediator.SetText(displayValue.ToString());
            }
        }
        else
        {
            Mediator.TextChanged.RemoveListener(TextChanged);
            SyncSource();
            Mediator.TextChanged.AddListener(TextChanged);
        }
    }

    private void SyncSource()
    {
        string value = this.source.getValueString();
        Mediator.SetText(value);
    }

    private void TextChanged(string newValue)
    {
        if (BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.BindingToSourceOneWay )
        {
            source.setFromValueString(Mediator.GetTextUnformatted());
        }
    }

}
