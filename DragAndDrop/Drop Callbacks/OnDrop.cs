using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDrop : DropCallback
{
    public UnityEvent OnDropCommit;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public override void OnPlacement(GameObject placedObject)
    {
        OnDropCommit.Invoke();
    }

    public override void OnPointerExit()
    {
        OnExit.Invoke();
    }

    public override void OnTempPlacement(GameObject ghostData)
    {
        OnEnter.Invoke();
    }
}
