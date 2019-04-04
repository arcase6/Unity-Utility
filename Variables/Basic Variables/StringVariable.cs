using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/StringVariable")]
public class StringVariable : ScriptableObject {
    public string InitialValue;
    public string Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
