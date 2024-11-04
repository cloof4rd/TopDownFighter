using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2Int minMaxCharacterIndex = new Vector2Int(1, 9);
    public CinemachineTargetGroup targetGroup;
    public SpriteRenderer mapRenderer;

    public Role[] playerRoleArr;

    public bool isDebugPlayer1;
    public int debugPlayer1Index = 1;

    private bool isGamePlaying = true;
    private float gameTime;
    public float GetGameTime()
    {
        return gameTime;
    }

    private void Awake()
    {
        Instance = this;
        var mapIndex = PlayerPrefs.GetInt(ConstConfig.MAP_SELECT_KEY, 1);
        this.mapRenderer.sprite = Resources.Load<Sprite>($"{ConstConfig.MAP_RESOURCE_PATH}{mapIndex}");
    }

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
        var role = playerRoleArr[playerIndex - 1];
        role.Init(selectedCharacterIndex);
        BattleManager.Instance.InitRoleBattleInfo(role, playerIndex);
        role.OnDie += GameOver;
    }

    private void GameOver()
    {
        isGamePlaying = false;
        BattleManager.Instance.FinishBattle();
        //foreach (var role in playerRoleArr)
        //{
        //    role.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        //}
    }

    private void FixedUpdate()
    {
        if (!isGamePlaying)
            return;
        gameTime += Time.fixedDeltaTime;
    }
}
