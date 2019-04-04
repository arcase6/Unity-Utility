using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector4Variable", menuName = "Variables/Vector4Variable")]
public class Vector4Variable : ScriptableObject {
    public Vector4 InitialValue;
    public Vector4 Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
