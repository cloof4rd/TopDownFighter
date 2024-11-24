using UnityEngine;
using UnityEngine.SceneManagement;
using YogiGameCore.Utils;

public class PausePanel : MonoBehaviour
{
    InputController[] controllers;
    bool isShow = false;
    public int ExitSceneIndex = 1;
    public static PausePanel Instance;
    private void Awake()
    {
        Instance = this;
        controllers = GameObject.FindObjectsOfType<InputController>();
        InputController.onPause += TriggerPausePanel;
    }
    private void OnDestroy()
    {
        InputController.onPause -= TriggerPausePanel;
        InputController.onContinueGame -= Hide;
        InputController.onRestartGame -= RestartBattle;
        InputController.onExitGame -= EnterRoleSelectScene;
        Time.timeScale = 1;
    }
    void TriggerPausePanel()
    {
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
