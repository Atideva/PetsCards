using System;
using System.Collections;
using DG.Tweening;
using game.cards.interfaces;
using UnityEngine;

namespace game.cards.abilityVfx
{
    public class BlackScreenFocus : CardAbilityVFX
    {
        [SerializeField] SpriteRenderer blackScreen;
        [SerializeField] Color blackColor;
        [SerializeField] float fadeTime;
        [SerializeField] float blackTime;
        [SerializeField] float zOffset = -1;
        [SerializeField] private float cardImageSize = 1.5f;

        protected override void OnVfxPlay()
        {
            StartCoroutine(PlayVfx());
        }

        protected override void Reset()
        {
            blackScreen.color = Color.clear;
            Debug.Log("AbilityVFX reset", this);
        }


        IEnumerator PlayVfx()
        {
            if(Card1==null) Debug.LogError("NUUUUUUUUUUUUUL CARD");
            var zPos = Card1.transform.position.z;

            MoveByZ(blackScreen.transform, zPos + zOffset);
            var checkRoutine = StartCoroutine(KeepCardPosition(zPos + zOffset * 2));

            yield return null;
            ScaleAnimation(Card1.MainArt.transform);
            ScaleAnimation(Card2.MainArt.transform);

            yield return blackScreen
                .DOColor(blackColor, fadeTime)
                .WaitForCompletion();

            UseAbility();
            yield return new WaitForSeconds(blackTime);

            yield return blackScreen
                .DOColor(Color.clear, fadeTime * 1.5f)
                .WaitForCompletion();

            StopCoroutine(checkRoutine);
            MoveByZ(Card1.transform, zPos);
            MoveByZ(Card2.transform, zPos);
            Finish();
        }

        void ScaleAnimation(Transform t) => t
            .DOScale(cardImageSize, 0.45f)
            .OnComplete(() => t
                .DOScale(1, 0.45f)
                .SetDelay(0.7f));

        static void MoveByZ(Transform t, float offset) => t.DOLocalMoveZ(offset, 0); // += new Vector3(0, 0, offset);
        // var pos = t.transform.position ;
        // pos = new Vector3(pos.x, pos.y, offset);
        // t.transform.position = pos;

        IEnumerator KeepCardPosition(float offsetByZ)
        {
            while (true)
            {
                CheckPos(offsetByZ, Card1);
                CheckPos(offsetByZ, Card2);
                yield return new WaitForSeconds(0.1f);
            }
        }

        void CheckPos(float offsetByZ, Card card)
        {
            var cardPos = card.transform.position;
            if (!(Math.Abs(cardPos.z - offsetByZ) > 0.1f)) return;

            cardPos = new Vector3(cardPos.x, cardPos.y, offsetByZ);
            card.transform.position = cardPos;
        }
    }
}