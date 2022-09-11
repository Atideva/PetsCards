using System.Collections.Generic;
using game.managers;
using UnityEngine;

namespace game.cards.managers
{
    public class CardSpawnVfx : MonoBehaviour
    {
        [SerializeField] ParticleObject spawnVfx;
        [SerializeField] ParticleObject deSpawnVfx;
 
        [SerializeField] Vector3 spawnOffset;
        [SerializeField] Vector3 deSpawnOffset;
        [SerializeField] float spawnSize;
        [SerializeField] float deSpawnSize;

        [SerializeField] int poolPrewarm = 16;
        [SerializeField] ParticlePool spawnPool;
        [SerializeField] ParticlePool deSpawnPool;
        public List<GameObject> saved = new();

        void Start()
        {
            spawnPool.Init(spawnVfx, poolPrewarm);
            deSpawnPool.Init(deSpawnVfx, 5);
            Events.Instance.OnSpawn += OnSpawn;
            Events.Instance.OnCombine += OnCombine;
            //    Events.Instance.OnDeSpawn += OnDeSpawn;
        }

        void OnCombine(Card card1, Card card2)
        {
            var size = card1.transform.localScale.x;
            var v = deSpawnPool.Get();
            v.transform.position = card1.transform.position + deSpawnOffset;
            v.transform.localScale = new Vector3(size * deSpawnSize, size * deSpawnSize, size * deSpawnSize);
        }

        void OnSpawn(Card card, float size)
        {
            var v = spawnPool.Get();
            v.transform.position = card.transform.position + spawnOffset;
            v.transform.localScale = new Vector3(size * spawnSize, size * spawnSize, size * spawnSize);
        }

        void OnDeSpawn(Card card, float size)
        {
            var v = deSpawnPool.Get();
            v.transform.position = card.transform.position + deSpawnOffset;
            v.transform.localScale = new Vector3(size * deSpawnSize, size * deSpawnSize, size * deSpawnSize);
        }
    }
}