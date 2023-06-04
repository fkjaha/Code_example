using System;
using UnityEngine;

[Serializable]
public abstract class KeyNamePair<T> where T: ScriptableObject
{
    public string GetName => name;
    public T GetSearchKey => searchKey;

    [SerializeField] private string name;
    [SerializeField] private T searchKey;
}
