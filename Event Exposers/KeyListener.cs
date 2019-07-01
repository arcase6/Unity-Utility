using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    public KeyCode Key = KeyCode.Return;
    public UnityEvent OnPress;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(Key)){
            OnPress.Invoke();
        }       
    }
}
