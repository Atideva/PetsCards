 
public class ParticleObject : PoolObject
{
    void OnDisable() => ReturnToPool();
 
}