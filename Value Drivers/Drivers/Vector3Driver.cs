using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vector3Driver : Driver<Vector3,Vector3>
{

    [SerializeField]
    [HideInInspector]
    Vector3 offset = Vector3.zero;
    public Vector3 Offset{
        get{
            return offset;
        }
        set{
            offset = value;
            this.UpdateFlag = true;
        }
    }


    public override Vector3 GetTargetValue()
    {
        if(BindingSources.Count == 1)
            return BindingSources[0].getValueVector3() + offset;
        else if(BindingSources.Count > 1){
            Vector3 sum = Vector3.zero;
            foreach(IBindingSource source in BindingSources){
                sum += source.getValueVector3();
            }
            sum = sum / BindingSources.Count;
            return sum + offset;
        }
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");            
    }

    public override List<Vector3> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueVector3()).ToList();
    }
}

