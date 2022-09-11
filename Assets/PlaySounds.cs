using __PUBLISH_v1.Scripts;
using game.cards;
using game.managers;
using systems.audio_manager.audio_Event;
using systems.audio_manager.audio_Manager;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    SoundData PairMiss => GameManager.Instance.Config.Sound.PairMiss;
    SoundData PairSuccess => GameManager.Instance.Config.Sound.PairSuccess;
    void Start()
    {
        Events.Instance.OnPairSuccess += OnPairSuccess;
        Events.Instance.OnPairMiss += OnPairMiss;
    }
    
    void Sound(SoundData sound) => AudioManager.Instance.PlaySound(sound);
    void OnPairMiss(Card c1, Card c2) => Sound(PairMiss);
    void OnPairSuccess(Card arg1, Card arg2) => Sound(PairSuccess);
}