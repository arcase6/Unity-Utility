using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Driver<T> : MonoBehaviour, IListener, ISerializationCallbackReceiver
{
    protected bool UpdateFlag;

    public Component DriveTarget;
    public string TargetProperty;


    [SerializeField]
    private List<BindingSourceData> BindingSourcesRaw = new List<BindingSourceData>();
    public List<IBindingSource> BindingSources;


    public DriverMode ModeOfOperation;

    [HideInInspector]
    [SerializeField]
    private ScriptableObject DriverEvaluatorSerializable;

    public DriverEvaluator<T> DriverEvaluator;
    private System.Action<T> SetTargetProp;

    public virtual void Start()
    {

        if (!SetupPropertyDelegates())
            OnDisable();
    }

    private void OnEnable()
    {
        foreach (IBindingSource source in BindingSources)
        {
            source.AddListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (IBindingSource source in BindingSources)
        {
            source.RemoveListener(this);
        }
    }

    public void EditorUpdate()
    {
        if (BindingSources.Count > 0)
        {
            if (this.SetupPropertyDelegates())
            {
                UpdateFlag = true;
                LateUpdate();
            }
            else
            {
                Debug.Log("Failed to setup target setter delegates");
            }
        }
        else
        {
            Debug.Log("There are no binding sources set");
        }
    }

    protected abstract T GetContextualValue();

    public abstract T GetSourceValue();

    public abstract List<T> GetSourceValues();

    public System.Type GetAllowedType()
    {
        return typeof(T);
    }

    public void SetUpdateFlag(bool value)
    {
        this.UpdateFlag = value;
    }

    public List<object> GetSourceValuesAsObjects()
    {
        return this.BindingSources.Select(b => b.getValueAsObject()).ToList();
    }

    public void Notify()
    {
        this.UpdateFlag = true;

    }

    private void LateUpdate()
    {
        try
        {
            if (UpdateFlag)
            {
                UpdateFlag = false;
                switch (this.ModeOfOperation)
                {
                    case DriverMode.Identity:
                        this.SetTargetProp(GetSourceValue());
                        break;
                    case DriverMode.UseEvaluater:
                        this.SetTargetProp(this.DriverEvaluator.Evaluate(GetSourceValues(), this.GetContextualValue()));
                        break;
                    case DriverMode.UseEvaluaterComplexSources:
                        this.SetTargetProp((T)this.DriverEvaluator.Evaluate(GetSourceValuesAsObjects(), this.GetContextualValue()));
                        break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to set Drive target" + e.Message);
        }
    }

    public bool SetupPropertyDelegates()
    {
        if (this.DriveTarget == null || this.TargetProperty == null)
            return false;
        try
        {
            System.Reflection.PropertyInfo info = DriveTarget.GetType().GetProperty(TargetProperty);
            if (info.PropertyType != typeof(T))
                throw new System.InvalidOperationException("The source is not of type " + typeof(T).ToString());
            this.SetTargetProp = (System.Action<T>)System.Delegate.CreateDelegate(typeof(System.Action<T>), DriveTarget, info.GetSetMethod());

        }
        catch
        {
            if (TargetProperty == null || TargetProperty == "")
                Debug.Log("Faild to bind to drive target.There is no Target Property specified.");
            else
                Debug.Log("Failed to bind to drive target :" + TargetProperty + ". Make sure that the property exists and is type " + typeof(T).Name, this);
            return false;
        }

        return true;
    }

    public void OnBeforeSerialize()
    {
        this.DriverEvaluatorSerializable = this.DriverEvaluator;
        if(BindingSources != null && BindingSources.Count > 0){
        BindingSourcesRaw = BindingSources.Select(b =>
        {
            BindingSourceType sourceType = (b as BindingSourceMonobehaviour != null) ? BindingSourceType.MonoBehaviour : BindingSourceType.ScriptableObject;
            return new BindingSourceData() { ObjectReference = b as Object, ReferenceType = sourceType };
        }).ToList();
        }
        else{
            BindingSourcesRaw = new List<BindingSourceData>();
        }
    }

    public void OnAfterDeserialize()
    {
        this.DriverEvaluator = DriverEvaluatorSerializable as DriverEvaluator<T>;
        BindingSources = BindingSourcesRaw.Select(b => b.ObjectReference as IBindingSource).ToList();
    }
}
