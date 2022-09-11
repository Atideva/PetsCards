using System.Collections;
using System.Collections.Generic;
using systems.audio_manager.audio_Event;
using UnityEngine;

[CreateAssetMenu(fileName = "Music config", menuName = "Data/Music config")]
public class MusicConfig : ScriptableObject
{
    [Header("Music")]
    [SerializeField] SoundData musicMainMenu;
    [SerializeField] SoundData musicCommon;
    [SerializeField] SoundData musicTimer;
    [SerializeField] SoundData musicLives;
    [SerializeField] SoundData musicPvp;
    public SoundData MusicPvp => musicPvp;

    public SoundData MusicMainMenu => musicMainMenu;
    public SoundData GetCLip(LevelMode mode)
    {
        return mode switch
        {
            LevelMode.None => musicCommon,
            LevelMode.Timer => musicTimer,
            LevelMode.Lives => musicLives,
            _ => null
        };
    }
}
