using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloatToVector3Driver : DriverContextSensitive<float, Vector3>
{
    public Vector3 Offset;
    public Vector3Bool WriteToggles;

    private void Reset() {
        Offset = Vector3.zero;
        WriteToggles = Vector3Bool.FalseVector;
        DriveTarget = null;
        PostProcessor = null;
        this.ResetSourceList();
    }

    

    protected override Vector3 GenerateDriveValue(Vector3 currentTargetValue)
    {
        float sourcesAverage = 0f;
        if(SourceCount >= 1){
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                float value = source.RuntimeBindingSource.getValueFloat();
                if(source.IsInverted) value *= -1;
                sourcesAverage += value;
            }
            if(this.AverageSourceValues)
                sourcesAverage /= SourceCount;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        if(WriteToggles.x)
            currentTargetValue.x = sourcesAverage;
        if(WriteToggles.y)
            currentTargetValue.y = sourcesAverage;
        if(WriteToggles.z)
            currentTargetValue.z = sourcesAverage;
        
        currentTargetValue += Offset;
        return currentTargetValue;
    }


    
}
