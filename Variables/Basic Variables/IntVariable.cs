using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variables/IntVariable")]
public class IntVariable : ScriptableObject {
    public int InitialValue;
    public int Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
