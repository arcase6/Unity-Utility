using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour , IListener
{
    
    public BooleanBindingVariable toggleVariable;

    [SerializeField]
    private MonoBehaviour target;
    public MonoBehaviour Target{
        get => target;
        set{
            target = value;
            toggleTarget = target?.gameObject ?? this.gameObject;

        }
    }
    private GameObject toggleTarget;
    // Start is called before the first frame update
    void Start()
    {
        toggleTarget = target?.gameObject ?? this.gameObject;
        toggleTarget.SetActive(!toggleVariable.Value);
    }


    private void OnEnable() {
        toggleVariable.AddListener(this);
    }
    private void OnDisable() {
        toggleVariable.RemoveListener(this);
    }

    public void Notify()
    {
        toggleTarget.SetActive(!toggleVariable.Value);
    }
}
