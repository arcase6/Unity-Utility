using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Driver<T,U> : MonoBehaviour, IListener, ISerializationCallbackReceiver
{

    public bool RunOnEnable = true;
    protected bool UpdateFlag;

    public Object DriveTarget;
    public string TargetProperty;

    public bool AverageSourceValues;

    public List<BindingSourceData> BindingSourcesSerializable = new List<BindingSourceData>();
    
    public IEnumerable<IBindingSource> BindingSources{
        get{
            return BindingSourcesSerializable.Select(b => b.RuntimeBindingSource);
        }
    }

    public int SourceCount{
        get{return BindingSourcesSerializable.Count;}
    }



    [HideInInspector]
    [SerializeField]
    private UnityEngine.Object PostProcessorSerializable;
    public PostProcessor<U> PostProcessor;


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
        this.UpdateFlag = RunOnEnable;
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


    public abstract U GenerateDriveValue();



    public void SetUpdateFlag(bool value)
    {
        this.UpdateFlag = value;
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
                if(this.PostProcessor != null)
                     value = PostProcessor.Proccess(value);
                this.SetTargetProp(value);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to set Drive target: " + e.Message);
        }
    }


    //made virtual so that a driver with context can
    //create a getter delegate needed for context
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

    //a helper method that is used in defining a Reset
    //method in a base class
    public void ResetSourceList(){
        this.BindingSourcesSerializable.Clear();
    }

    public void OnBeforeSerialize()
    {
        
        this.PostProcessorSerializable = this.PostProcessor as UnityEngine.Object;
    }

    public void OnAfterDeserialize()
    {
        foreach(BindingSourceData bindingSource in BindingSourcesSerializable){
            bindingSource.RuntimeBindingSource = bindingSource.ObjectReference as IBindingSource;
        }

        this.PostProcessor = this.PostProcessorSerializable as PostProcessor<U>;

    }
}
