using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector4Driver : Driver<Vector4>
{

    [SerializeField]
    [HideInInspector]
    Vector4 offset = Vector4.zero;
    public Vector4 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector4 GetSourceValue()
    {
        if(BindingSources.Count == 1)
            return BindingSources[0].getValueVector4() + offset;
        else if(BindingSources.Count > 1){
            Vector4 sum = Vector4.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector4();
            }
            sum = sum / BindingSources.Count;
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    public override List<Vector4> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueVector4()).ToList();
    }

    protected override Vector4 GetContextualValue()
    {
        return offset;
    }
}

