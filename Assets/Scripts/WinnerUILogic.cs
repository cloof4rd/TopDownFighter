using DG.Tweening;
using UnityEngine;

public class WinnerUILogic : MonoBehaviour
{
    public RectTransform player1UITrans, player2UITrans;
    public Vector3 offset = new Vector3(0, 10, 0);
    private float timer;
    public float scaleEndValue = .5f;
    public float scaleAnimTime = 1;
    private void Awake()
    {
        this.transform.DOScale(scaleEndValue, scaleAnimTime)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.playerRoleArr == null)
            return;

        var roles = GameManager.Instance.playerRoleArr;
        var winnerRole = (roles[0].hp > 0) ? roles[0] : roles[1];
        if (winnerRole.playerIndex == 0) //winner
        {
            this.transform.position = player1UITrans.position + offset;
        }
        else
        {
            this.transform.position = player2UITrans.position + offset;
        }

        timer += Time.deltaTime;
    }
}
