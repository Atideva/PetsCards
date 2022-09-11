using System.Collections;
using System.Collections.Generic;
using game.cards.data;
using UnityEngine;

[CreateAssetMenu(fileName = "Card List", menuName = "Data/Card List")]
public class CardList : ScriptableObject
{
    [SerializeField] List<CardData> cards = new();
    public IReadOnlyList<CardData> Cards => cards;
}
