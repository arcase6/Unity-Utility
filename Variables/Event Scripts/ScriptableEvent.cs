using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Mass Visualizer/ScriptableEvent")]
public class ScriptableEvent : ScriptableObject, IObservable {
    private List<IListener> Listeners = new List<IListener>();   

    public void NotifyChange(){
        for(int i = Listeners.Count - 1; i >= 0; i--)
            Listeners[i].Notify();
    }

    public void AddListener(IListener listener){
        if(!Listeners.Contains(listener))
            Listeners.Add(listener);
    }

    public void RemoveListener(IListener listener){
        Listeners.Remove(listener);
    }
}