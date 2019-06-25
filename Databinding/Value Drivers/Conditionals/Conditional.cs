using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conditional<T> : MonoBehaviour
{
    public abstract bool CheckValue(T value);
}
