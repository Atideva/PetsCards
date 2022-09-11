using systems.audio_manager.audio_Event;
using UnityEngine;

namespace game.cards
{
    [CreateAssetMenu(fileName = "NewAbility", menuName = "Cards/New Card Ability", order = 3)]
    public class AbilityConfig : ScriptableObject
    {
        [SerializeField] AbilityType type;
        [SerializeField] AbilityActivator prefab;
        [SerializeField] string abilityName;
        [SerializeField] [TextArea] string description;
        [SerializeField] SoundData sound;
        public string Description => description;
        public string AbilityName => abilityName;

        public AbilityActivator Prefab => prefab;
        public AbilityType Type => type;

        public SoundData Sound => sound;
    }
}