using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnActivate : MonoBehaviour
{
    public int frameDelay = 0;
    private int remainingFrames;

    public UnityEvent Actions;
    // Start is called before the first frame update
    private void OnEnable() {
        if(frameDelay <= 0){
        Actions.Invoke();
        remainingFrames = -1;
        }
        else{
            remainingFrames = frameDelay - 1;
        }
    }

    private void Update(){
        if(remainingFrames == 0){
            remainingFrames--;
            Actions.Invoke();
        }
        else if(remainingFrames > 0){
            remainingFrames--;
        }
        
    }

}
