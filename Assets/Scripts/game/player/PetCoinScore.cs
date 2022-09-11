using System.Collections;
using __PUBLISH_v1.Scripts;
using DG.Tweening;
using game.cards;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.player
{
    public class PetCoinScore : MonoBehaviour
    {
        public bool enableScirpt;
        [Header("Setup")]
        [SerializeField] SoundData sound;
        [SerializeField] Transform moveToPosition;
        [Header("Settings")]
        [SerializeField] float spawnDelay;
        [SerializeField] float moveTime;
        [SerializeField] [Range(0f, 0.5f)] float moveTimeRange;
        [Header("Pool")]
        [SerializeField] PetCoinObject prefab;
        [SerializeField] PetCoinPool pool;
        [Header("CoinUI")]
        [SerializeField] TextMeshProUGUI coinTxt;
        [SerializeField] Image coinImage;
        [Header("Animation")]
        [SerializeField] float pumpSize = 1.3f;
        [SerializeField] float pumpTime = 0.2f;
        [Header("DEBUG")]
        [SerializeField] int petCoins;
        Vector3 MovePosition => _camera.ScreenToWorldPoint(moveToPosition.position);
        float RandomMoveTime => moveTime * Random.Range(1 - moveTimeRange, 1 + moveTimeRange);

        public int TotalCoins => petCoins;
        public int poolPrewarm = 16;
        Camera _camera;

        void Start()
        {
            if (!enableScirpt) return;
 
            pool.Init(prefab,poolPrewarm);
            petCoins = 0;
            _camera = Camera.main;
            Events.Instance.OnDeSpawn += OnDespawn;
            RefreshText(petCoins);
        }

        void OnDespawn(Card c, float size) 
            => StartCoroutine(CreatePetCoinVfx(c.transform.position, size, RandomMoveTime, spawnDelay));

        public bool coinPerPairOnly;
        int isPair;
        IEnumerator CreatePetCoinVfx(Vector3 pos, float scale, float moveTime, float delay)
        {
            yield return new WaitForSeconds(delay);
            var petCoin = pool.Get();
            petCoin.transform.position = pos;
            petCoin.transform.localScale = new Vector3(scale, scale, scale);
            yield return petCoin.transform.DOMove(MovePosition, moveTime).WaitForCompletion();
            if (coinPerPairOnly)
            {
                isPair++;
                if (isPair == 2)
                {
                    isPair = 0;
                    yield break;
                }
            }
            AddCoin(1);
            RefreshText(petCoins);
            PumpAnimation();
            petCoin.gameObject.SetActive(false);
            AudioManager.Instance.PlaySound(sound);
        }

        void AddCoin(int amount)
        {
            petCoins += amount;
            GameManager.Instance.UserResourceSave.AddPetCoin(amount);
        }

        void PumpAnimation()
        {
            coinImage.transform.DOKill();
            coinImage.transform
                .DOScale(pumpSize, pumpTime / 2)
                .OnComplete(()
                    => coinImage.transform.DOScale(1, pumpTime / 2));
        }

        void RefreshText(int amoun) 
            => coinTxt.text = amoun.ToString();
    }
}