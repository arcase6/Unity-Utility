using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
public class FloatVariable : ScriptableObject {
    public float InitialValue;
    public float Value;

    private void Awake() {
        Value = InitialValue;    
    }
}  