using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInputRows : MonoBehaviour
{
    public TextMeshProUGUI displayNameText;
    public Image[] images;
    public char[] charKeys;
    public Sprite[] spriteValues;
    private Dictionary<char, Sprite> keyMap;
    private void Awake()
    {
        keyMap = new Dictionary<char, Sprite>();
        for (int i = 0; i < charKeys.Length; i++)
        {
            keyMap.Add(charKeys[i], spriteValues[i]);
        }
        Clear();
    }

    private void Clear()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
    }
    public void InitSkill(string displayName, string skillInput)
    {
        Clear();
        displayNameText.text = displayName;
        for (int i = 0; i < skillInput.Length; i++)
        {
            var c = skillInput[i];
            var displaySprite = keyMap[c];
            var img = images[i];
            img.gameObject.SetActive(true);
            img.sprite = displaySprite;
        }
    }
}
