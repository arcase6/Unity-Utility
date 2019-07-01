using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrefabInitializer : MonoBehaviour
{
    public abstract void Process(GameObject instantiatedObject);
}
