using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterThanInt : IntConditional
{
    public int Threshold;
    public override bool CheckValue(int value)
    {
        return value > Threshold;
    }
}
