 
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    IPool _pool;
    public void InitPool(IPool pool) => _pool = pool;
    public void ReturnToPool() => _pool.ReturnToPool(this);
}
