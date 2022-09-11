using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game visual", menuName = "Data/Game visual")]
public class GameVisual : ScriptableObject
{
    [SerializeField] Sprite iconGold;
    [SerializeField] Sprite iconPetCoin;
    [SerializeField] Sprite iconGem;

    public Sprite IconGold => iconGold;

    public Sprite IconPetCoin => iconPetCoin;

    public Sprite IconGem => iconGem;

    public Sprite GetSprite(VisualType type)
    {
        return type switch
        {
            VisualType.IconGold => iconGold,
            VisualType.IconPetCoin => iconPetCoin,
            VisualType.IconGem => iconGem,
            _ => null
        };
    }
}

public enum VisualType
{
    IconGold = 0,
    IconPetCoin = 1,
    IconGem = 2
}