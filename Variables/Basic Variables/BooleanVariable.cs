using UnityEngine;

[CreateAssetMenu(menuName = "Variables/BooleanVariable")]
public class BooleanVariable : ScriptableObject {
    public bool InitialValue; 
    public bool Value;
    public bool AttemptToggleOn(){
        if(Value)
            return false;
        Value = true;
        return true;
    }

    private void Awake() {
        Value = InitialValue;
    }
}