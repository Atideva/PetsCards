 
public class PetCoinObject : PoolObject
{
    void OnDisable() => ReturnToPool();
 
}
