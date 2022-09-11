using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Colors", menuName = "Data/Colors")]
public class ColorConfig : ScriptableObject
{
    [SerializeField] List<Color> colors = new();

    public IReadOnlyList<Color> Colors => colors;
}
