﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloatDriver : Driver<float>
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

    public override float GetSourceValue()
    {
        if(this.BindingSources.Count == 1)
            return BindingSources[0].getValueFloat();
        else if(this.BindingSources.Count > 1)
            return BindingSources.Average(b => b.getValueFloat());
        else
            throw new System.NullReferenceException("There are no sources defined for this driver.");
        
    }

    public override List<float> GetSourceValues()
    {
        return BindingSources.Select(b => b.getValueFloat()).ToList();
    }

    protected override float GetContextualValue()
    {
        return 0f;
    }
}
