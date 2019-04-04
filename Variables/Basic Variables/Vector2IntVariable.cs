using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2IntVariable", menuName = "Variables/Vector2IntVariable")]
public class Vector2IntVariable : ScriptableObject {
    public Vector2Int InitialValue;
    public Vector2Int Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
