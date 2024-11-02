
using System.Collections.Generic;
using UnityEngine;

public class ConstConfig
{
    public const int ArtRoleSpriteColumnCount = 15;
    public static string[] ArtResourcePathArr =
    {
        "RoleArts/1Spearman",
        "RoleArts/2DayWalker",
        "RoleArts/9Broadsword"
    };
    public const int BATTLE_SCENE_INDEX = 1;
    public static Vector2Int RoleMinMaxIndex = new Vector2Int(1, 9);
    public static int LayerMaskBlock = LayerMask.GetMask("Block");
    public static int LayerMaskPlayer = LayerMask.GetMask("Player");
}
