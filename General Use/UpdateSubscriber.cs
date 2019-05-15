using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateSubscriber : MonoBehaviour, ISerializationCallbackReceiver
{
    public bool UpdateUsingFixedPeriod;
    public float UpdatePeriod;
    private float Timer;
    [SerializeField]
    private List<UnityMethodData> MethodDefinitions = new List<UnityMethodData>();
    private Dictionary<UnityMethodData, System.Action> CachedActions = new Dictionary<UnityMethodData, System.Action>();

    public bool AddMethod(Component targetComponent, string propertyName)
    {
        UnityMethodData newItem = new UnityMethodData(targetComponent, propertyName);
        CacheMethodDelegate(newItem);
        if(MethodDefinitions.Any(md => md == newItem))
            return false;
        MethodDefinitions.Add(newItem);
        return true;
    }
    public bool RemoveMethod(Component targetComponent, string propertyName)
    {
        return this.RemoveMethod(new UnityMethodData(targetComponent, propertyName));
    }

    public bool RemoveMethod(UnityMethodData methodSpecification)
    {
        if (this.CachedActions.ContainsKey(methodSpecification))
            CachedActions.Remove(methodSpecification);
        bool wasItemPresent = MethodDefinitions.Any(md => md == methodSpecification);
        MethodDefinitions.RemoveAll(md => md == methodSpecification);
        return wasItemPresent;
    }


    private void Reset()
    {
        UpdateUsingFixedPeriod = false;
        UpdatePeriod = .1f;
        MethodDefinitions = new List<UnityMethodData>();
        CachedActions = new Dictionary<UnityMethodData, System.Action>();
    }

    private void OnEnable()
    {
        Timer = 0f;
    }

   

    private bool CacheMethodDelegate(UnityMethodData methodData)
    {
        try
        {
            System.Type objectType = methodData.TargetComponent.GetType();
            System.Reflection.MethodInfo methodInfo = objectType.GetMethod(methodData.MethodName, new System.Type[0]);
            System.Action action = System.Delegate.CreateDelegate(typeof(System.Action), methodData.TargetComponent, methodInfo) as System.Action;
            if (action != null)
                CachedActions.Add(methodData, action);
            else
                return false;
        }
        catch
        {
            string MethodName = methodData.MethodName ?? "Null MethodName";
            Debug.Log("Failed to create delegate for " + MethodName + " in Component : " + methodData.TargetComponent.name);
            return false;
        } 
        return true;
    }

    void Update()
    {
        if (!UpdateUsingFixedPeriod || UpdatePeriod == 0)
            CallMethods();
        else
        {
            Timer += Time.deltaTime;
            if (Timer >= UpdatePeriod)
            {
                CallMethods();
                Timer = Timer % UpdatePeriod;
            }
        }
    }

    private void CallMethods()
    {
        for (int index = MethodDefinitions.Count - 1; index >= 0; index--)
        {
            UnityMethodData methodData = MethodDefinitions[index];
            try
            {
                System.Action action = CachedActions[methodData];
                action();
            }
            catch
            {
                try
                {
                    if (this.CacheMethodDelegate(methodData))
                    {
                        System.Action action = CachedActions[methodData];
                        action();
                        continue;
                    }
                }
                catch { }
                RemoveBrokenMethodData(index);
            }
        }
    }

    private void RemoveBrokenMethodData(int index)
    {
        UnityMethodData methodData = MethodDefinitions[index];
        if (CachedActions.ContainsKey(methodData))
            CachedActions.Remove(methodData);
        MethodDefinitions.RemoveAt(index);
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        foreach (UnityMethodData methodData in MethodDefinitions)
        {
            if (!this.CachedActions.ContainsKey(methodData))
                CacheMethodDelegate(methodData);
        }
    }
}
