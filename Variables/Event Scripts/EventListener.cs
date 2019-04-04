using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour, IListener
{
    
    public ScriptableEvent Event;
    public UnityEvent OnEventRaised;

    private void Awake() {
        
    }
    public void Notify()
    {
        OnEventRaised.Invoke();
    }


    private void OnEnable() {
        Event.AddListener(this);
    }

    private void OnDisable() {
        Event.RemoveListener(this);    
    }
}
