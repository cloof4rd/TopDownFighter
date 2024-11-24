using System;
using TMPro;
using UnityEngine;

public class RoleDetailPanel : MonoBehaviour
{
    public TextMeshProUGUI role1NameText, role2NameText;
    public void SetDisplay(string role1Name, string role2Name)
    {
        role1NameText.text = role1Name;
        role2NameText.text = role2Name;
    }

    public void SetDisplay(int playerIndex, string name)
    {
        if (playerIndex == 1)
        {
            role1NameText.text = name;
        }
        else if (playerIndex == 2)
        {
            role2NameText.text = name;
        }
    }
}
