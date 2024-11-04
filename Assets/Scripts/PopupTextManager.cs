using System.Collections.Generic;
using UnityEngine;

public class PopupTextManager : MonoBehaviour
{
    public static Dictionary<string, DynamicTextData> dataDic = new Dictionary<string, DynamicTextData>();
    public bool isPrintToConsole = false;
    public void Awake()
    {
        dataDic = new Dictionary<string, DynamicTextData>();
        DynamicTextData[] allData = Resources.LoadAll<DynamicTextData>("PopupText");
        foreach (DynamicTextData data in allData)
        {
            dataDic.Add(data.name, data);
            if (isPrintToConsole)
                print($"load:{data.name}");
        }
    }

    public static void PopupDamage(Vector2 position, string text)
    {
        DynamicTextManager.CreateText2D(position, text, dataDic["Damage"]);
    }
    public static void PopupMiss(Vector2 position, string text)
    {
        DynamicTextManager.CreateText2D(position, text, dataDic["Miss"]);
    }
    public static void PopupBlock(Vector2 position, string text)
    {
        DynamicTextManager.CreateText2D(position, text, dataDic["Block"]);
    }
}
