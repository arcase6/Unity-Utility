using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject where T: MonoBehaviour
{
    public List<T> Items = new List<T>();

    public void Add(T setMember)
    {
        if (!Items.Contains(setMember)) Items.Add(setMember);
    }

    public void Remove(T setMember)
    {
        if (Items.Contains(setMember)) Items.Remove(setMember);
    }
}
