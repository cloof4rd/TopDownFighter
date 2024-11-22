using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        var saveIndex = (selectedCharacterIndex - 1);
        switchRoleToggleArr[saveIndex].isOn = true;
        OnlyShowTarget(saveIndex);


        dieBtn.onClick.AddListener(() =>
        {
            foreach (var role in roles)
            {
                if (role.gameObject.activeInHierarchy)
                    role.ReceiveDamage(100);
            }
        });

        takeDamageBtn.onClick.AddListener(() =>
        {
            foreach (var role in roles)
            {
                if (role.gameObject.activeInHierarchy)
                    role.ReceiveDamage(0);
            }
        });

        backToMenuBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(ConstConfig.MENU_SCENE_INDEX);
        });
    }

    private void OnlyShowTarget(int tmpI)
    {
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

}
