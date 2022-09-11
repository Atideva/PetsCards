using System.Collections.Generic;
using Misc;
using UnityEngine;


public class Pool<T> : MonoBehaviour, IPool where T : PoolObject
{
    T _prefab;
    readonly Queue<T> _queue = new();

    public void Init(T prefab, int prewarmCount = 0)
    {
        _prefab = prefab;
        for (var i = 0; i < prewarmCount; i++) Create().gameObject.SetActive(false);
    }

    public T Get() =>
        _queue.Count <= 0
            ? Create()
            : _queue.Dequeue().With(t => t.gameObject.SetActive(true));

    T Create()
        => Instantiate(_prefab, transform).With(p => p.InitPool(this));

    public void ReturnToPool(IPoolObject poolObject)
    {
        if (poolObject is not T obj) return;
        _queue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}