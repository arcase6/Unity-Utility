using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPrefabFilter : MonoBehaviour
{
    public List<string> allowedPrefabNames = new List<string>();
    public bool invert;

    public void Reset(){
        DropPrefab[] dropPanels = GetComponents<DropPrefab>();
        foreach(DropPrefab dp in dropPanels){
            if(dp.filter == null)
                dp.filter = this;
        }
    }

    public bool isAllowed(string prefabName){
        foreach(string name in allowedPrefabNames){
            if(prefabName.Contains(name))
                return !invert; 
        }
        return invert;
    }
}
