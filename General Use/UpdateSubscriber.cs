using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateSubscriber : MonoBehaviour
{
    public bool UpdateUsingFixedPeriod;
    public float UpdatePeriod;
    private float Timer;
    public List<MethodInfoSerializable> Methods;
    private System.Action[] Actions;

    private void Reset() {
        UpdateUsingFixedPeriod = false;
        UpdatePeriod = .1f;
        Methods = null;
    }

    private void OnEnable() {
        Timer = 0;
    }

    void Start(){

        List<System.Action> actions = new List<System.Action>(Methods.Count);

        for(int i = 0; i < Methods.Count;i++){
            try{
            Object referenceObject = Methods[i].ObjectReference;
            string MethodName = Methods[i]. MethodName;
            System.Type objectType = referenceObject.GetType();
            System.Reflection.MethodInfo methodInfo = objectType.GetMethod(MethodName,new System.Type[0]);
            System.Action action = System.Delegate.CreateDelegate(typeof(System.Action),referenceObject,methodInfo) as System.Action;
            if(action != null) actions.Add(action);
            }catch(System.Exception e){
                string MethodName = Methods[i].MethodName ?? "Null MethodName";
                Debug.Log("Failed to find: " + MethodName);
                Debug.Log(e.Message);
            }
        }
        Actions = actions.ToArray();

    }

    void Update()
    {
        if (!UpdateUsingFixedPeriod || UpdatePeriod == 0)
            CallMethods();
        else{
            Timer +=Time.deltaTime;
            if(Timer >= UpdatePeriod){
                CallMethods();
                Timer = Timer % UpdatePeriod;
            }
        }
    }

    private void CallMethods()
    {
        foreach (System.Action action in Actions)
        {
            action.Invoke();
        }
    }
}
