using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecimalVariable", menuName = "Variables/DecimalVariable")]
public class DecimalVariable : ScriptableObject {
    public decimal InitialValue;
    public decimal Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
