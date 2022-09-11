using DG.Tweening;
using UnityEngine;

namespace game.cards.glow
{
    public class Glow : MonoBehaviour
    {
        [SerializeField] bool useVfx;
        [SerializeField] GameObject container;
        [SerializeField] ParticleSystem frameVfx;
        [SerializeField] ParticleSystem sparkVfx;
        [SerializeField] bool useSprite;
        [SerializeField] SpriteRenderer glowSprite;

        [SerializeField] float spriteAlpha = 0.3f;
        float baseAlpha = 1f;
        const float GLOW_ALPHA = 0.85f;
        float _frameSpeed;
        float _sparksSpeed;
        float _frameHalfSpeed = 5f;

        void Awake()
        {
            var main = frameVfx.main;
            _frameSpeed = main.simulationSpeed;
            var sparksMain = sparkVfx.main;
            _sparksSpeed = sparksMain.simulationSpeed;
            // Disable();
            container.SetActive(false);
            glowSprite.enabled = false;
        }

        public void SetColor(Color frameColor, Color sparksColor)
        {
            if (useVfx)
            {
                frameColor.a = GLOW_ALPHA;
                SetColor(sparkVfx, sparksColor);
                SetColor(frameVfx, frameColor);
            }

            if (useSprite)
            {
                baseAlpha = frameColor.a;
                glowSprite.color = frameColor;
            }
        }

        public void Enable() => Play();

        public void Disable(float delay = 0) => Invoke(nameof(Stop), delay);


        public void Half(Color sparksClr)
        {
            if (useVfx)
            {
                var emission = frameVfx.emission;
                emission.enabled = false;
                var frame = frameVfx.main;
                frame.simulationSpeed = _frameHalfSpeed;

                sparksClr.a = 0.5f;
                var spark = sparkVfx.main;
                spark.simulationSpeed = _sparksSpeed / 3;
                spark.startColor = sparksClr;
            }

            if (useSprite)
            {
                glowSprite.enabled = true;
                sparksClr.a = 0.5f;
                glowSprite.color = sparksClr;
            }
        }

        void SetColor(ParticleSystem particle, Color color)
        {
            if (!useVfx) return;
            var main = particle.main;
            main.startColor = color;
        }

        void Play()
        {
            if (useVfx)
            {
                container.SetActive(true);
                var frame = frameVfx.emission;
                frame.enabled = true;
                var frameMain = frameVfx.main;
                frameMain.simulationSpeed = _frameSpeed;
                var spark = sparkVfx.emission;
                spark.enabled = true;
                var sparkMain = sparkVfx.main;
                sparkMain.simulationSpeed = _sparksSpeed;
            }

            if (useSprite)
            {
                glowSprite.enabled = true;
                glowSprite.DOKill();
                glowSprite.DOFade(spriteAlpha * baseAlpha, 0.2f);
            }
        }

        void Stop()
        {
            if (useVfx) container.SetActive(false);
            if (useSprite)
            {
                glowSprite.DOKill();
                glowSprite.DOFade(0, 0.2f).OnComplete(()
                    => glowSprite.enabled = false);
            }
            // var frame = frameVfx.emission;
            // frame.enabled = false;
            // var spark = sparkVfx.emission;
            // spark.enabled = false;
        }
    }
}