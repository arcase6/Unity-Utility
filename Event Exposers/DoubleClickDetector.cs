using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickDetector : MonoBehaviour, IPointerDownHandler
{
    float Clicked = 0f;
    float ClickTime = 0f;
    float MaxClickDelay = 0.5f;
    public UnityEngine.Events.UnityEvent DoubleClick;
    
    public void OnPointerDown(PointerEventData eventData)
    {

        Clicked++;
        if(Clicked == 1)ClickTime = Time.time;
        else if(Clicked > 1){
            if( Time.time - ClickTime < MaxClickDelay){
                ClickTime = Time.time;
                DoubleClick.Invoke();
            }
            else{
                Clicked = 1;
                ClickTime = Time.time;
            }
        }
    }
}
