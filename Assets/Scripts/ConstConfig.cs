
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
    public const string MAP_RESOURCE_PATH = "Map/Dungeon Arena";

    public const int MENU_SCENE_INDEX = 0;
    public static int ROLE_SELECTION_SCENE_INDEX = 1;
    public const int MAP_SELECT_SCENE_INDEX = 2;
    public const int BATTLE_SCENE_INDEX = 3;
    public static int TRAINING_SCENE_INDEX = 4;

    public static Vector2Int RoleMinMaxIndex = new Vector2Int(1, 9);
    public static int LayerMaskBlock = LayerMask.GetMask("Block");
    public static int LayerMaskPlayer = LayerMask.GetMask("Player");

    public const float BattleBGMSound = .25f;
    public const float CommonSound = .5f;

    // PlayerPrefs Key
    public const string MAP_SELECT_KEY = "MapSelect";
}
