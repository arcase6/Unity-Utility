using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleVariable", menuName = "Variables/DoubleVariable")]
public class DoubleVariable : ScriptableObject {
    public double InitialValue;
    public double Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
