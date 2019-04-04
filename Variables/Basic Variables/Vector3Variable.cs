using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "Variables/Vector3Variable")]
public class Vector3Variable : ScriptableObject {
    public Vector3 InitialValue;
    public Vector3 Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
