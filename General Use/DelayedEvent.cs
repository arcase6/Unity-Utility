using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    public UnityEvent Event;
    public void Trigger(int frames){
        if(frames == 0){
            Event.Invoke();
        }
        else{
            StartCoroutine("WaitAndCall",frames);
        }
    }

    private IEnumerator WaitAndCall(int frames){
        while(frames > 0){
            frames--;
            yield return null;
        }
        Event.Invoke();
    }


}
