using TMPro;
using UnityEngine;

public class PlayerBattleInfoView : MonoBehaviour
{
    public TextMeshProUGUI timeText, beHitCountText, tryBlockCountText,
        blockSuccessCountText;

    public void SetUp(BattleInfo battleInfo)
    {
        var time = GameManager.Instance.GetGameTime();
        int minutes = Mathf.FloorToInt(time / 60);
        int seconeds = Mathf.FloorToInt(time % 60);
        timeText.text = $"{minutes}:{seconeds}";
        beHitCountText.text = battleInfo.beHitCount.ToString("000");
        tryBlockCountText.text = battleInfo.tryToBlockCount.ToString("000");
        blockSuccessCountText.text = battleInfo.blockSuccessCount.ToString("000");
    }
}
