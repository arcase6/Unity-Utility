using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropCallback : MonoBehaviour
{
    public abstract void OnTempPlacement(GameObject ghostData);

    public abstract void OnPlacement(GameObject placedObject);

    public abstract void OnPointerExit();
}
