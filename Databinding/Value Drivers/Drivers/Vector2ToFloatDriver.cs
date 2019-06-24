using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vector2ToFloatDriver : Driver<Vector2, float>
{

    [SerializeField]
    [HideInInspector]
    float offset = 0f;
    public float Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Vector2 mask = new Vector2(1f, 0f);
    public Vector2 Mask {
        get{
            return mask;
        }
        set{
            mask= value;
            this.UpdateFlag = true;
        } }


    private void Reset() {
        Offset = 0;
        Mask = new Vector2(1f,0f);
        DriveTarget = null;
        PostProcessor = null;
        this.ResetSourceList();
    }

    public override float GenerateDriveValue()
    {
        float sum = 0f;
        if(SourceCount >= 1){
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                Vector2 value = source.RuntimeBindingSource.getValueVector2() * Mask;
                if(source.IsInverted) value *= -1;
                sum += value.x + value.y;
            }
            if(this.AverageSourceValues)
                sum /= SourceCount;
            return sum;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
    }
}
