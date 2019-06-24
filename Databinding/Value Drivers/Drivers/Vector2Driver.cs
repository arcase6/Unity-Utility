using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector2Driver : Driver<Vector2,Vector2>
{

    [SerializeField]
    [HideInInspector]
    Vector2 offset = Vector2.zero;
    public Vector2 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector2 GenerateDriveValue()
    {
        if(SourceCount == 1)
            return BindingSources.First().getValueVector2() + offset;
        else if(SourceCount> 1){
            Vector2 sum = new Vector2(0,0);
            foreach(BindingSourceData source in this.BindingSourcesSerializable){
                Vector2 value = source.RuntimeBindingSource.getValueVector2();
                if(source.IsInverted) value *= -1;
                sum += value;
            }
            if(this.AverageSourceValues)
                sum = sum / BindingSources.Count();
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    
}

