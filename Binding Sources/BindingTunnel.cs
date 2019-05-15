using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingTunnel : BindingTunnelManual
{
    public bool UpdateOnInterval = false;
    public float Timer = 0f;
    public float RefreshInterval = .1f;

    protected override void OnEnable()
    {
        Timer = 0f;
    }


    public void Update(){
        if(!UpdateOnInterval || RefreshInterval == 0f){
            this.PerformSourceCheck();
        }
        else{
            Timer += Time.deltaTime;
            if(Timer > RefreshInterval){
                Timer = Timer % RefreshInterval;
                this.PerformSourceCheck();
            }
        }
    }
    
}
