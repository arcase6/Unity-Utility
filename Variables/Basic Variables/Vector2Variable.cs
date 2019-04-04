using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Variable", menuName = "Variables/Vector2Variable")]
public class Vector2Variable : ScriptableObject {
    public Vector2 InitialValue;
    public Vector2 Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  
