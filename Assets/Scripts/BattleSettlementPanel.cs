using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class BattleSettlementPanel : MonoBehaviour
{
    public Button restartBtn, exitBattleBtn;
    public PlayerBattleInfoView player1BattleInfoView, player2BattleInfoView;

    private void Awake()
    {
        restartBtn.onClick.AddListener(RestartBattle);
        exitBattleBtn.onClick.AddListener(EnterRoleSelectScene);
        BattleManager.Instance.OnFinishBattle += Popup;
        gameObject.SetActive(false);
    }

    private void Popup(List<BattleInfo> battleInfos)
    {
        if (battleInfos.Count != 2)
        {
            Debug.LogError($"Error Battle Info Count:{battleInfos.Count}");
            return;
        }

        player1BattleInfoView.SetUp(battleInfos[0]);
        player2BattleInfoView.SetUp(battleInfos[1]);
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn.gameObject);
    }

    void RestartBattle()
    {
        SceneManager.LoadScene(ConstConfig.BATTLE_SCENE_INDEX);
    }
    void EnterRoleSelectScene()
    {
        SceneManager.LoadScene(ConstConfig.ROLE_SELECTION_SCENE_INDEX);
    }
}
