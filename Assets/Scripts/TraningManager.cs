using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YogiGameCore.Utils.MonoExtent;

public class TraningManager : MonoBehaviour
{
    public Role rolePrefab;
    public Role currentRole;

    public Toggle[] switchRoleToggleArr;
    public Button dieBtn, takeDamageBtn, backToMenuBtn;
    public CinemachineTargetGroup group;
    public InputController controller;

    public List<Role> roles;

    public Role Enemy;
    public int EnemyIndex = 1;

    private int CurrentDisplayIndex;

    private void Start()
    {
        Enemy.Init(EnemyIndex);
        Enemy.hp = float.MaxValue / 2.0f;

        roles = new List<Role>();
        for (int i = ConstConfig.RoleMinMaxIndex.x; i <= ConstConfig.RoleMinMaxIndex.y; i++)
        {
            roles.Add(InitRoleByIndexWithOutSetup(i));
        }

        for (int i = 0; i < switchRoleToggleArr.Length; i++)
        {
            var index = i;
            switchRoleToggleArr[i].onValueChanged.AddListener((v) =>
            {
                if (v)
                    OnlyShowTarget(index);
            });
        }

        int selectedCharacterIndex = PlayerPrefs.GetInt($"selectedCharacterP0");
        OnlyShowTarget(selectedCharacterIndex - 1);


        dieBtn.onClick.AddListener(() =>
        {
            TakeDamage();
        });

        takeDamageBtn.onClick.AddListener(() =>
        {
            Die();
        });

        InputController.onPlayDieAnim += Die;
        InputController.onPlayTakeDamageAnim += TakeDamage;
        InputController.onSwitchNextRole += ShowNextRole;
        InputController.onSwitchPrevRole += ShowPrevRole;
    }

    private void OnlyShowTarget(int tmpI)
    {
        CurrentDisplayIndex = tmpI;
        switchRoleToggleArr[CurrentDisplayIndex].isOn = true;
        foreach (var otherRole in roles)
        {
            otherRole.gameObject.SetActive(false);
        }

        var role = roles[tmpI];
        role.gameObject.SetActive(true);
        group.m_Targets[0].target = role.transform;
        controller.SwitchRole(role);
    }

    private Role InitRoleByIndexWithOutSetup(int selectedCharacterIndex)
    {
        Role role = GameObject.Instantiate(rolePrefab);
        role.Init(selectedCharacterIndex);
        role.gameObject.SetActive(false);
        return role;
    }

    [Button]
    public void ShowNextRole()
    {
        var newIndex = CurrentDisplayIndex + 1;
        newIndex %= switchRoleToggleArr.Length;
        OnlyShowTarget(newIndex);
    }
    [Button]
    public void ShowPrevRole()
    {
        var newIndex = CurrentDisplayIndex - 1;
        if (newIndex < 0)
            newIndex = switchRoleToggleArr.Length - 1;
        OnlyShowTarget(newIndex);
    }
    private void Die()
    {
        foreach (var role in roles)
        {
            if (role.gameObject.activeInHierarchy)
                role.ReceiveDamage(100);
        }
    }
    private void TakeDamage()
    {
        foreach (var role in roles)
        {
            if (role.gameObject.activeInHierarchy)
                role.ReceiveDamage(0);
        }
    }

}
