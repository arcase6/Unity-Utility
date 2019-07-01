using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChild : DropCallback
{
    public string ChildNameToActivate;

    public override void OnPlacement(GameObject placedObject)
    {

    }

    public override void OnPointerExit()
    {
        
    }

    public override void OnTempPlacement(GameObject ghostData)
    {
        int index = -1;
        for (int i = 0; i < ghostData.transform.childCount; i++)
        {
            GameObject child = ghostData.transform.GetChild(i).gameObject;
            bool active = child.name == ChildNameToActivate;
            if (active)
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            for (int i = 0; i < ghostData.transform.childCount; i++)
            {
                GameObject child = ghostData.transform.GetChild(i).gameObject;
                child.SetActive(i == index);
            }
        }
    }
}
