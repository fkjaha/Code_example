using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour where T: UnityEngine.Object
{
    [SerializeField] private T instancePrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int poolSize;

    private Queue<T> _pool;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _pool = new Queue<T>();
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Enqueue(Instantiate(instancePrefab, poolParent));
        }
    }

    public T GetObject()
    {
        T result = _pool.Dequeue();
        _pool.Enqueue(result);
        return result;
    }
}
