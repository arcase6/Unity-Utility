using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleToCircleDriver : DriverContextSensitive<float, Vector3>
{
    public Vector3Bool WriteToggles = new Vector3Bool(true,false,true);

    //if these need to change just use a driver
    [SerializeField]
    private Vector3 xAxis = new Vector3(1,0,0);
    public Vector3 XAxis{
        get{return xAxis * scale;}
        set{
            xAxis = value;
            UpdateFlag = true;
        }
    }
    [SerializeField]
    private Vector3 yAxis = new Vector3(0,0,1);
    public Vector3 YAxis{
        get{return yAxis * scale;}
        set{
            yAxis = value;
            UpdateFlag = true;
        }
    }
    [SerializeField]    
    private Vector3 center; 
    public Vector3 Center{
        get{return center;}
        set{
            center = value;
            UpdateFlag = true;
        }
    }

    [SerializeField]
    private float scale = 1f;
    public float Scale{
        get{return scale;}
        set{
            scale = value;
            UpdateFlag = true;
        }
    }
    
    [SerializeField]
    private bool useRadians;

    public bool UseRadians{
        get{return useRadians;}
        set{
            useRadians = value;
            UpdateFlag = true;
        }
    }

        

    protected override Vector3 GenerateDriveValue(Vector3 currentTargetValue)
    {
        //get the aggregated float 
        float totalAngle = 0f;
        foreach(BindingSourceData bd in BindingSourcesSerializable){
            if(bd.IsInverted)
                totalAngle -= bd.RuntimeBindingSource.getValueFloat();
            else
                totalAngle += bd.RuntimeBindingSource.getValueFloat();
        }
        if(this.AverageSourceValues)
            totalAngle /= SourceCount;
        
        if(!UseRadians)
            totalAngle *= Mathf.Deg2Rad;

        Vector3 pointMappedToCircle = Center;
        pointMappedToCircle += XAxis * Mathf.Sin(totalAngle);
        pointMappedToCircle += YAxis * Mathf.Cos(totalAngle);


        if(WriteToggles.x)
            currentTargetValue.x = pointMappedToCircle.x;
        if(WriteToggles.y)
            currentTargetValue.y = pointMappedToCircle.y;
        if(WriteToggles.z)
            currentTargetValue.z = pointMappedToCircle.z;

        return currentTargetValue;
    }



}
