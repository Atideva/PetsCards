using systems.audio_manager.audio_Event;
using UnityEngine;

namespace game.managers
{
    [CreateAssetMenu(fileName = "Sound config", menuName = "Data/Sound config")]
    public class SoundConfig : ScriptableObject
    {

        [Header("Sounds")]
        [SerializeField] SoundData cardsLayout;
        [SerializeField] SoundData cardFlip;
        [SerializeField] SoundData cardFlipBack;
        [SerializeField] SoundData roundComplete;
        [SerializeField] SoundData liveHeal;
        [SerializeField] SoundData liveDamage;
        [SerializeField] SoundData scoreUp;
        [SerializeField] SoundData comboUp;
        [SerializeField] SoundData scoreUpByCombo;
        [SerializeField] SoundData comboTrail;
        [SerializeField] SoundData coinTrail;
        [SerializeField] SoundData pairMiss;
        [SerializeField] SoundData pairSuccess;
        [SerializeField] SoundData sliderCardCollect;
        [SerializeField] SoundData sliderRoundComplete;

        public SoundData CardsLayout => cardsLayout;

        public SoundData CardFlip => cardFlip;

        public SoundData CardFlipBack => cardFlipBack;

        public SoundData RoundComplete => roundComplete;

        public SoundData LiveHeal => liveHeal;

        public SoundData LiveDamage => liveDamage;

        public SoundData ScoreUp => scoreUp;

        public SoundData ScoreUpByCombo => scoreUpByCombo;

        public SoundData ComboTrail => comboTrail;

        public SoundData CoinTrail => coinTrail;

        public SoundData ComboUp => comboUp;



        public SoundData PairSuccess => pairSuccess;

        public SoundData PairMiss => pairMiss;

        public SoundData SliderCardCollect => sliderCardCollect;

        public SoundData SliderRoundComplete => sliderRoundComplete;
    }
}