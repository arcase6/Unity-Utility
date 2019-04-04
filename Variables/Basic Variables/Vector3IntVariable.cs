using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3IntVariable", menuName = "Variables/Vector3IntVariable")]
public class Vector3IntVariable : ScriptableObject {
    public Vector3Int InitialValue;
    public Vector3Int Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
