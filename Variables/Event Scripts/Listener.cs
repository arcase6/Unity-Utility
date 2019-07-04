using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour, IListener, ISerializationCallbackReceiver
{
    
    [HideInInspector]
    [SerializeField]
    private Object SourceSerialized;

    public IObservable Source;

    public UnityEvent OnNotify;

    private void Awake() {
        
    }
    public void Notify()
    {
        OnNotify.Invoke();
    }


    private void OnEnable() {
        Source.AddListener(this);
    }

    private void OnDisable() {
        Source.RemoveListener(this);    
    }

    public void OnBeforeSerialize()
    {
        this.SourceSerialized = Source as Object;
    }

    public void OnAfterDeserialize()
    {
        this.Source = SourceSerialized as IObservable;
    }
}
