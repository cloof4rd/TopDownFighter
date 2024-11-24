using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1000)]
public class BattleSettlementPanel : MonoBehaviour
{
    public PlayerBattleInfoView player1BattleInfoView, player2BattleInfoView;

    private bool isPlayer1Ready, isPlayer2Ready;

    public InputController inputController1, inputController2;

    private void Awake()
    {
        isPlayer1Ready = isPlayer2Ready = false;
        BattleManager.Instance.OnFinishBattle += Popup;
        gameObject.SetActive(false);
    }
    private void Start()
    {
        if (inputController1.playerIndex == 1)
        {
            var tmp = inputController1;
            inputController1 = inputController2;
            inputController2 = tmp;
        }

        inputController1.onSubmit += () =>
        {
            player1BattleInfoView.Ready();
            CheckAllPlayerReady().Forget();
        };
        inputController2.onSubmit += () =>
        {
            player2BattleInfoView.Ready();
            CheckAllPlayerReady().Forget();
        };
        inputController1.onCancel += EnterRoleSelectScene;
        inputController2.onCancel += EnterRoleSelectScene;
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
    }
    private async UniTaskVoid CheckAllPlayerReady()
    {
        await UniTask.WaitForSeconds(player1BattleInfoView.readyAnimTime + 0.1f);
        if (player1BattleInfoView.isReady && player2BattleInfoView.isReady)
            RestartBattle();
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
