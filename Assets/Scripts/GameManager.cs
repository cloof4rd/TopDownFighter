using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector2Int minMaxCharacterIndex = new Vector2Int(1, 9);
    public CinemachineTargetGroup targetGroup;

    public Role[] playerRoleArr;

    public bool isDebugPlayer1;
    public int debugPlayer1Index = 1;

    private void Start()
    {
        if (isDebugPlayer1)
        {
            playerRoleArr[0].Init(debugPlayer1Index);
        }
        else
        {
            InitCharacterIndexByPlayerIndex(1);
        }
        InitCharacterIndexByPlayerIndex(2);
    }
    private void InitCharacterIndexByPlayerIndex(int playerIndex)
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt($"selectedCharacterP{playerIndex}");
        playerRoleArr[playerIndex - 1].Init(selectedCharacterIndex);
    }
}
