using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Binding : MonoBehaviour, IListener
{
    public bool InitializeFromSource = false;
    [HideInInspector]
    public BindingMode BindingMode;

    public IBindingSource Source;

    [HideInInspector]
    [SerializeField]
    private BindingSourceData SourceRaw = new BindingSourceData(){ReferenceType = BindingSourceType.MonoBehaviour}; //included so unity can serialize properly
    private float cachedSourceValue = 0.0f;
    private UIMediatorLegacy Mediator;

    private void Reset()
    {
        if (GetComponent<UIMediatorLegacy>() == null)
        {
            gameObject.AddComponent(typeof(UIMediatorLegacy));
        }
        SourceRaw = new BindingSourceData(){ReferenceType = BindingSourceType.MonoBehaviour,ObjectReference = null};
    }

    private void Awake()
    {
        Mediator = GetComponent<UIMediatorLegacy>();
    }

    public virtual void OnBeforeSerialize()
    {
        SourceRaw = new BindingSourceData() { ObjectReference = Source as Object, ReferenceType = BindingSourceType.MonoBehaviour };
        if (Source as BindingSourceScriptableObject != null)
            SourceRaw.ReferenceType = BindingSourceType.ScriptableObject;
        if (Source != null && Source.LockBindingMode)
            this.BindingMode = Source.PrefferedMode;
        
    }

    public virtual void OnAfterDeserialize()
    {
        if (SourceRaw != null)
            Source = SourceRaw.ObjectReference as IBindingSource;
    }



    private void OnEnable()
    {
        if(Source == null){
            OnAfterDeserialize();
        }
        cachedSourceValue = Source.getValueFloat();
        Source.AddListener(this);
        if (!Mediator.isInitialiazed)
            Mediator.Start();
        Mediator.TextChanged.AddListener(TextChanged);
        if (InitializeFromSource)
            SyncSource();
    }

    private void OnDisable()
    {
        Source.RemoveListener(this);
        Mediator.TextChanged.RemoveListener(TextChanged);
    }

    public virtual void Notify()
    {
        if (BindingMode == BindingMode.BindingToSourceOneWay)
        {
            string sourceValue = this.Source.getValueString();
            string bindingValue = this.Mediator.GetText();
            if (sourceValue != bindingValue)
                Source.setFromValueString(bindingValue);
        }
        else if (BindingMode == BindingMode.OffsetFromSource)
        {
            float sourceValue = Source.getValueFloat();
            float difference = sourceValue - cachedSourceValue;
            if (Mathf.Abs(difference) >= .01f)
            {
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
        string value = this.Source.getValueString();
        Mediator.SetText(value);
    }

    private void TextChanged(string newValue)
    {
        if (BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.BindingToSourceOneWay)
        {
            Source.setFromValueString(Mediator.GetTextUnformatted());
        }
    }

}
