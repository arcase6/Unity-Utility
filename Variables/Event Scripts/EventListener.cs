using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour, IListener
{
    [HideInInspector]
    public ScriptableEvent Source;
    public UnityEvent OnEventRaised;

    private void Awake() {
        
    }
    public void Notify()
    {
        OnEventRaised.Invoke();
    }


    private void OnEnable() {
        Source.AddListener(this);
    }

    private void OnDisable() {
        Source.RemoveListener(this);    
    }
}
