using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YogiGameCore.Utils.MonoExtent;

public class PlayerBattleInfoView : MonoBehaviour
{
    public TextMeshProUGUI timeText, beHitCountText, tryBlockCountText,
        blockSuccessCountText, roleNameText;

    public Image headIcon, classIcon;

    private RoleDescriptionConfig config => Resources.Load<RoleDescriptionConfig>("RoleDescriptionConfig");

    public RectTransform readyBG;
    public RectTransform readyTargetTrans;
    public float readyAnimTime = .1f;
    public bool isReady { get; private set; } = false;

    public GameObject WinnerGO, LoserGO;

    [Button]
    public void Ready()
    {
        if (isReady)
            return;
        isReady = true;
        readyBG.DOMove(readyTargetTrans.position, readyAnimTime);
    }
    private void SetIsWinner(bool isWinner)
    {
        WinnerGO.SetActive(isWinner);
        LoserGO.SetActive(!isWinner);
    }
    public void SetUp(BattleInfo battleInfo)
    {
        var time = GameManager.Instance.GetGameTime();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconeds = Mathf.FloorToInt(time % 60);
        var data = config.GetDeatilByIndex(battleInfo.roleIndex);
        roleNameText.text = data.Name;
        headIcon.sprite = data.headIcon;
        classIcon.sprite = data.ClassIcon;
        SetIsWinner(battleInfo.isWinner);

        timeText.text = $"{minutes}:{seconeds}";
        beHitCountText.text = battleInfo.beHitCount.ToString("000");
        tryBlockCountText.text = battleInfo.tryToBlockCount.ToString("000");
        blockSuccessCountText.text = battleInfo.blockSuccessCount.ToString("000");
    }
}
