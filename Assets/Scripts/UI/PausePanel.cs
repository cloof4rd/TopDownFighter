using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YogiGameCore.Utils;

public class PausePanel : MonoBehaviour
{
    InputController[] controllers;
    bool isShow = false;
    public int ExitSceneIndex = 1;
    public static PausePanel Instance;

    private int displayRoleIndex = -1;
    RoleConfig[] roleConfigs;
    public SkillInputRows[] rows;
    public TextMeshProUGUI displayRoleNameText;
    public bool isActiveRestartFuncion = true;

    private void Awake()
    {
        Instance = this;
        controllers = GameObject.FindObjectsOfType<InputController>();
        InputController.onPause += TriggerPausePanel;

        roleConfigs = Resources.LoadAll<RoleConfig>("Configs");
    }
    private void OnDestroy()
    {
        InputController.onPause -= TriggerPausePanel;
        InputController.onContinueGame -= Hide;
        InputController.onRestartGame -= RestartBattle;
        InputController.onExitGame -= EnterRoleSelectScene;
        Time.timeScale = 1;
    }
    void TriggerPausePanel(int roleIndex)
    {
        displayRoleIndex = roleIndex - 1;
        isShow = !isShow;
        if (isShow)
            Show();
        else
            Hide();
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        Time.timeScale = 0;
        controllers.ForEach(x =>
        {
            x.isBlockInput = true;
            x.SwitchToUIInput();
        });
        InputController.onRestartGame += RestartBattle;
        InputController.onExitGame += EnterRoleSelectScene;
        InputController.onContinueGame += Hide;
        this.gameObject.SetActive(true);
        isShow = true;

        var config = roleConfigs[displayRoleIndex];

        var roleName = config.roleName;
        displayRoleNameText.text = roleName;

        rows.ForEach(row => row.gameObject.SetActive(false));
        var currentRowIndex = 0;
        config.animSpeedConfig.data
            .Where(x => x.SkillInputKey.IsNotNullAndEmpty() && x.SkillInputKey != "X")
            .ForEach(data =>
            {
                var displayName = data.skillName;
                var displaySkillInput = data.SkillInputKey;
                // 显示技能名与技能按键信息
                rows[currentRowIndex].gameObject.SetActive(true);
                rows[currentRowIndex].InitSkill(displayName, displaySkillInput);
                currentRowIndex++;
            });
    }
    public void Hide()
    {
        Time.timeScale = 1;
        controllers.ForEach(x =>
        {
            x.isBlockInput = false;
            x.SwitchToCommonInput();
        });
        this.gameObject.SetActive(false);
        InputController.onContinueGame -= Hide;
        InputController.onRestartGame -= RestartBattle;
        InputController.onExitGame -= EnterRoleSelectScene;
        isShow = false;
    }



    public void RestartBattle()
    {
        if (!isActiveRestartFuncion)
            return;
        Time.timeScale = 1;
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }
    public void EnterRoleSelectScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(ExitSceneIndex);
    }
}
