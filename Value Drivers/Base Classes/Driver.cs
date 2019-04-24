using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Driver<T,U> : MonoBehaviour, IListener, ISerializationCallbackReceiver
{
    protected bool UpdateFlag;

    public Component DriveTarget;
    public string TargetProperty;

    public bool AverageSourceValues;

    public List<BindingSourceData> BindingSourcesFull = new List<BindingSourceData>();
    
    public IEnumerable<IBindingSource> BindingSources{
        get{
            return BindingSourcesFull.Select(b => b.RuntimeBindingSource);
        }
    }

    public int SourceCount{
        get{return BindingSourcesFull.Count;}
    }



    [HideInInspector]
    [SerializeField]
    private ScriptableObject DriverEvaluatorSerializable;

    public DriverEvaluator<T,U> DriverEvaluator;
    private System.Action<U> SetTargetProp;
    


    public virtual void Start()
    {

        if (!SetupPropertyDelegates())
            OnDisable();
    }

    private void OnEnable()
    {
        foreach (IBindingSource source in BindingSources)
        {
            source?.AddListener(this);
        }
        this.UpdateFlag = true;
    }

    private void OnDisable()
    {
        foreach (IBindingSource source in BindingSources)
        {
            source?.RemoveListener(this);
        }
    }

    public void EditorUpdate()
    {
        if (SourceCount > 0)
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


    public abstract U GetTargetValueStandard();



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

    [ExecuteInEditMode]
    private void LateUpdate()
    {
        try
        {
            if (UpdateFlag)
            {
                UpdateFlag = false;
                U value = GenerateDriveValue();
                this.SetTargetProp(value);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to set Drive target" + e.Message);
        }
    }

    protected virtual U GenerateDriveValue()
    {
        if(this.DriverEvaluator == null)
            return GetTargetValueStandard();
        else{
            throw new System.NotImplementedException(); //need to rewrite evaluators (they should now only have a single method)
        }
    }

    public virtual bool SetupPropertyDelegates()
    {
        if (this.DriveTarget == null || this.TargetProperty == null)
            return false;
        try
        {
            System.Reflection.PropertyInfo info = DriveTarget.GetType().GetProperty(TargetProperty);
            if (info.PropertyType != typeof(U))
                throw new System.InvalidOperationException("The target is not of type " + typeof(U).ToString());
            this.SetTargetProp = (System.Action<U>)System.Delegate.CreateDelegate(typeof(System.Action<U>), DriveTarget, info.GetSetMethod());

        }
        catch
        {
            if (TargetProperty == null || TargetProperty == "")
                Debug.Log("Failed to bind to drive target.There is no Target Property specified.");
            else
                Debug.Log("Failed to bind to drive target :" + TargetProperty + ". Make sure that the property exists and is type " + typeof(T).Name, this);
            return false;
        }

        return true;
    }

    public void ResetSourceList(){
        this.BindingSourcesFull.Clear();
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        foreach(BindingSourceData bindingSource in BindingSourcesFull){
            bindingSource.RuntimeBindingSource = bindingSource.ObjectReference as IBindingSource;
        }
    }
}
