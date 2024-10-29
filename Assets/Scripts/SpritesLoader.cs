using System.Collections.Generic;
using UnityEngine;

public static class SpritesLoader
{
    public static Dictionary<Direction, Sprite[]> LoadSprites(string spritesPath)
    {
        var result = new Dictionary<Direction, Sprite[]>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritesPath);
        if (sprites.Length < 120)
        {
            Debug.LogError($"Load Character Sprite Not Enough:{spritesPath}");
        }

        var readIndex = 0;
        for (int enumIndex = 0; enumIndex <= (int)Direction.NE; enumIndex++)
        {
            Sprite[] arr = new Sprite[15];
            for (int i = 0; i < 15; i++)
            {
                arr[i] = sprites[readIndex];
                readIndex++;
            }
            result.Add((Direction)enumIndex, arr);
        }
        return result;
    }
}
public enum Direction
{
    E = 0,
    SE,
    S,
    SW,
    W,
    NW,
    N,
    NE,
}
